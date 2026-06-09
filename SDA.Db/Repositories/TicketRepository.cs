using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    internal class TicketRepository(AppContext db) : ITicketRepository
    {
        public async Task<List<Ticket>?> GetAll()
        {
            return await db.Tickets
                .AsNoTracking()
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Topic)
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Answers)
                .AsSplitQuery()
                .ToListAsync();
        }


        public async Task<Ticket?> GetById(Guid id, bool track = false)
        {
            var tickets = track ? db.Tickets : db.Tickets.AsNoTracking();
            return await tickets.
                Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Topic)
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Answers)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task Create(Ticket ticket)
        {
            if (await GetById(ticket.Id) is not null)
            {
                throw new DuplicateWaitObjectException($"Билет с таким Id {ticket.Id} уже существует");
            }

            await db.AddAsync(ticket);
            await db.SaveChangesAsync();
        }


        public async Task Update(Ticket incomingTicket)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var ticket = await GetById(incomingTicket.Id, track: true);

                    if (ticket == null)
                        throw new KeyNotFoundException($"Билет с Id {incomingTicket.Id} не найден.");


                    ticket.Theme = incomingTicket.Theme;
                    ticket.Description = incomingTicket.Description;
                    ticket.Name = incomingTicket.Name;


                    var incomingQuestionIds = incomingTicket.TicketQuestions
                        .Where(x => x.QuestionId != Guid.Empty)
                        .Select(x => x.QuestionId)
                        .ToList();

                    var toRemove = ticket.TicketQuestions
                        .Where(x => !incomingQuestionIds.Contains(x.QuestionId))
                        .ToList();

                    foreach (var tq in toRemove)
                    {
                        ticket.TicketQuestions.Remove(tq);
                    }

                    foreach (var incomingTq in incomingTicket.TicketQuestions)
                    {
                        TicketQuestion? existingTq = null;
                        if (incomingTq.QuestionId != Guid.Empty)
                        {
                            existingTq = ticket.TicketQuestions.FirstOrDefault(x => x.QuestionId == incomingTq.QuestionId);
                        }

                        if (existingTq is not null)
                        {

                            existingTq.Order = incomingTq.Order;

                            var quest = existingTq.Question;
                            quest.Text = incomingTq.Question.Text;
                            quest.Explanation = incomingTq.Question.Explanation;
                            quest.ImageUrl = incomingTq.Question.ImageUrl;
                            quest.TopicId = incomingTq.Question.TopicId;


                            var incomingAnsIds = incomingTq.Question.Answers
                                .Where(x => x.Id != Guid.Empty)
                                .Select(x => x.Id)
                                .ToList();

                            var ansToRemove = quest.Answers.Where(x => !incomingAnsIds.Contains(x.Id)).ToList();
                            foreach (var ans in ansToRemove)
                            {
                                quest.Answers.Remove(ans);
                            }

                            // Обновляем/Добавляем ответы
                            foreach (var incomingAnswer in incomingTq.Question.Answers)
                            {
                                Answer? exAns = null;
                                if (incomingAnswer.Id != Guid.Empty)
                                {
                                    exAns = quest.Answers.FirstOrDefault(x => x.Id == incomingAnswer.Id);
                                }

                                if (exAns is not null)
                                {
                                    // Обновляем существующий ответ
                                    exAns.Text = incomingAnswer.Text;
                                    exAns.IsRight = incomingAnswer.IsRight;
                                    exAns.Order = incomingAnswer.Order;
                                }
                                else
                                {
                                    // Добавляем новый ответ в существующий вопрос
                                    quest.Answers.Add(incomingAnswer);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            ticket.TicketQuestions.Add(incomingTq);
                            await db.SaveChangesAsync();
                        }
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Ошибка обновления");
                }
            }
        }

        public async Task Delete(Guid id)
        {
            var ticket = await db.Tickets
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Topic)
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Answers)
                        .FirstOrDefaultAsync(x => x.Id == id);
            if (ticket is null)
                throw new KeyNotFoundException("Билет не найден");
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
        }
    }
}

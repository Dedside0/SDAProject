using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    internal class TicketRepository(AppContext ddd) : ITicketRepository
    {
        public async Task<List<Ticket>?> GetAll() => ddd.Tickets
            .AsNoTracking()
            .Include(x => x.TicketQuestions)
                .ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Answers)
            .AsSplitQuery()
            .ToList();


        public async Task<Ticket?> GetById(Guid id, bool track = false)
        {
            var tickets = track ? ddd.Tickets : ddd.Tickets.AsNoTracking();
            return await tickets.Include(x => x.TicketQuestions)
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

            await ddd.AddAsync(ticket);
            await ddd.SaveChangesAsync();
        }


        public async Task Update(Ticket incomingTicket)
        {


            var ticket = await GetById(incomingTicket.Id, track: true);



            if (ticket == null)
                throw new KeyNotFoundException($"Билет с Id {incomingTicket.Id} не найден.");

            ticket.Difficulty = incomingTicket.Difficulty;
            ticket.Description = incomingTicket.Description;
            ticket.Name = incomingTicket.Name;


            var incomingIds = incomingTicket.TicketQuestions.Select(x => x.QuestionId).ToList();
            var toRemove = ticket.TicketQuestions.Where(x => !incomingIds.Contains(x.QuestionId)).ToList();
            foreach (var tq in toRemove)
            {
                ticket.TicketQuestions.Remove(tq);
            }

            foreach (var incomingTq in incomingTicket.TicketQuestions)
            {
                var existingTq = ticket.TicketQuestions.FirstOrDefault(x => x.QuestionId == incomingTq.QuestionId);
                //Если вопрос уже существовал в этом билете то изменяем его
                if (existingTq is not null)
                {
                    var incomingAnsIds = incomingTq.Question.Answers.Select(x => x.Id).ToList();
                    var ansToRemove = existingTq.Question.Answers.Where(x => !incomingAnsIds.Contains(x.Id)).ToList();
                    foreach (var ans in ansToRemove)
                    {
                        existingTq.Question.Answers.Remove(ans);
                    }
                    existingTq.Order = incomingTq.Order;
                    var quest = existingTq.Question;
                    quest.Text = incomingTq.Question.Text;
                    quest.Exploration = incomingTq.Question.Exploration;
                    quest.ImageUrl = incomingTq.Question.ImageUrl;
                    foreach (var incomingAnswer in incomingTq.Question.Answers)
                    {
                        var exAns = existingTq.Question.Answers.FirstOrDefault(x => x.Id == incomingAnswer.Id);
                        if (exAns is not null)
                        {
                            exAns.Text = incomingAnswer.Text;
                            exAns.IsRight = incomingAnswer.IsRight;
                        }
                        else
                        {
                            existingTq.Question.Answers.Add(incomingAnswer);
                        }

                    }
                }
                else
                {
                    ticket.TicketQuestions.Add(incomingTq);
                }
            }

            await ddd.SaveChangesAsync();
        }
    }
}

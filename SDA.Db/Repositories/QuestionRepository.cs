using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    internal class QuestionRepository(AppContext dbContext) : IQuestionRepository
    {
        public List<Question>? GetAll() => dbContext.Questions
            .AsNoTracking()
            .Include(x=>x.Answers)
            .ToList();

        public Question? GetById(Guid id) => dbContext.Questions
            .AsNoTracking()
            .Include(x=>x.Answers)
            .FirstOrDefault(x=>x.Id==id);

        public async Task Create(Question question)
        {
            if ( GetById(question.Id) is not null)
            {
                await Update(question);
                return;
            }

            await dbContext.AddAsync(question);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Question question)
        {
            var existingQuestion = GetById(question.Id);

            if (existingQuestion == null)
            {
                await Create(question);
                return;
            }

            existingQuestion.Text = question.Text;
            existingQuestion.Answers = question.Answers;
            await dbContext.SaveChangesAsync();
        }
    }
}

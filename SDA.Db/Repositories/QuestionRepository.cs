using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    internal class QuestionRepository(AppContext dbContext) : IQuestionRepository
    {
        public List<Question>? GetAll() => dbContext.Questions.ToList();

        public async Task<Question?> GetById(Guid id) =>
            await dbContext.Questions.AsNoTracking()
            .Include(x => x.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);

        public async Task Create(Question question)
        {
            if (await GetById(question.Id) is not null)
            {
                await Update(question);
                return;
            }

            await dbContext.AddAsync(question);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Question question)
        {
            var existingQuestion = await GetById(question.Id);

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

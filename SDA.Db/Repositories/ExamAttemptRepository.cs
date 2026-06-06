using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    public class ExamAttemptRepository(AppContext dbContext) : IExamAttemptRepository
    {
        public void Create(ExamAttempt attempt)
        {
            dbContext.ExamAttempts.Add(attempt);
            dbContext.SaveChanges();
        }
        public ExamAttempt? GetById(Guid id)
        {
            return dbContext.ExamAttempts.Include(x=>x.User).Include(x=>x.QuestionAttempts)
                .FirstOrDefault(a => a.Id == id);
        }
        public List<ExamAttempt>? GetAllByUserId(Guid userId)
        {
            return dbContext.ExamAttempts.Include(x => x.User).Include(x => x.QuestionAttempts)
                .Where(a => a.UserId == userId).ToList();
        }

    }
}

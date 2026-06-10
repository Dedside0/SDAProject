using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    public class QuestionAttemptRepository(AppContext dbContext) : IQuestionAttemptRepository
    {
        public void Create(QuestionAttempt questionAttempt)
        {
            dbContext.QuestionAttempts.Add(questionAttempt);
            dbContext.SaveChanges();
        }

        public List<QuestionAttempt> GetListById (Guid attemptId)
        {
            return dbContext.QuestionAttempts
                .Where(qa => qa.ExamAttemptId == attemptId)
                .ToList();
        }

        public List<QuestionAttempt> GetUserAttempts(Guid userId)
        {
            return dbContext.QuestionAttempts.Include(x=>x.User).Include(x=>x.Question)
                .Where(x => x.UserId == userId)
                .ToList();
        }
    }
}

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
                .Where(qa => qa.AttemptId == attemptId)
                .ToList();
        }
    }
}

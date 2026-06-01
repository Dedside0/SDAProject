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
        public ExamAttempt GetById(Guid id)
        {
            return dbContext.ExamAttempts
                .FirstOrDefault(a => a.Id == id);
        }
    }
}

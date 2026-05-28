using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Models
{
    public class QuestionAttempt
    {
        public Guid Id { get; set; }
        public Guid AttemptId { get; set; }       
        public Guid QuestionId { get; set; }       
        public Guid? SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }       
        public string Topic { get; set; }       
        public DateTime AnsweredAt { get; set; } 
        public ExamAttempt Attempt { get; set; }
        public Question Question { get; set; }
    }
}

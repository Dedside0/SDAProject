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
        public bool IsCorrect { get; set; }       
        public string Topic { get; set; }       
        public DateTime AnsweredAt { get; set; } 
        
        public Guid ExamAttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedAnswerId { get; set; }
        public Guid UserId { get; set; }
        public ExamAttempt ExamAttempt { get; set; }
        public Question Question { get; set; }
        public Answer SelectedAnswer { get; set; }
        public User User { get; set; }
    }
}

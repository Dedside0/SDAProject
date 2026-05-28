using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Models
{
    public class ExamAttempt
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool IsPassed { get; set; }
        public int TimeSpentSeconds { get; set; }
        public int WrongCount { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
    }
}

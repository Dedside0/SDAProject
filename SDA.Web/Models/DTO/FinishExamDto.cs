namespace SDAProject.Controllers
{
    public partial class ExamController
    {
        public class FinishExamDto
        {
            public Guid TicketId { get; set; }
            public int TimeSpentSeconds { get; set; }
            public string QuestionAttemptsJson { get; set; } = "";
        }


    }
}

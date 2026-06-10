namespace SDAProject.Controllers
{
    public partial class ExamController
    {
        public class QuestionAttemptDto
        {
            public Guid QuestionId { get; set; }
            public Guid? SelectedAnswerId { get; set; }
            public bool IsCorrect { get; set; }
            public string? Topic { get; set; }
            public DateTime AnsweredAt { get; set; }
            public int TimeSpentSeconds { get; set; }
        }


    }
}

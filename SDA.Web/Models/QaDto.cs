namespace SDAProject.Controllers
{
    public partial class GeneratedExamController
    {
        public class QaDto
        {
            public Guid      QuestionId       { get; set; }
            public Guid?     SelectedAnswerId { get; set; }
            public bool      IsCorrect        { get; set; }
            public string    Topic            { get; set; } = "";
            public DateTime  AnsweredAt       { get; set; }
        }
    }
}

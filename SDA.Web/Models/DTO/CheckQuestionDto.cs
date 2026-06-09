namespace SDAProject.Controllers
{
    public partial class ExamController
    {
        public class CheckQuestionDto
        {
            public Guid QuestionId { get; set; }
            public Guid AnswerId { get; set; }
        }


    }
}

namespace SDAProject.Controllers
{
    public partial class GeneratedExamController
    {
        public class FinishDto
        {
            public bool   IsPassed             { get; set; }
            public int    TimeSpentSeconds      { get; set; }
            public string QuestionAttemptsJson  { get; set; } = "";
        }
    }
}

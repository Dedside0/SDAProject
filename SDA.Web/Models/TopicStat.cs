namespace SDA.Web.Models
{
    public partial class ExamResultVm
    {
        public class TopicStat
        {
            public string Topic { get; set; } = "";
            public int Correct { get; set; }
            public int Total { get; set; }
            public int Accuracy =>
                Total > 0 ? (int)Math.Round(Correct * 100.0 / Total) : 0;
        }
    }
}
namespace SDA.Web.Models
{
    public class QuestionVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public string? ImageUrl { get; set; }
        public string? Topic { get; set; }
        public List<AnswerVm> Answers { get; set; }
    }
}

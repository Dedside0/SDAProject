namespace SDA.Web.Models
{
    public class TicketVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<TicketQuestionVm> TicketQuestions { get; set; }
    }

    public class TicketQuestionVm
    {
        public int Order { get; set; }
        public QuestionVm Question { get; set; }
    }

    public class QuestionVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public string? ImageUrl { get; set; }
        public string? Topic { get; set; }
        public List<AnswerVm> Answers { get; set; }
    }

    public class AnswerVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsRight { get; set; }
    }
}

namespace SDA.Web.Models.DTO
{
    
    public class SaveTicketDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Difficulty { get; set; }
        public string? Description { get; set; }
        public List<SaveTicketQuestionDto> TicketQuestions { get; set; }
    }

    public class SaveTicketQuestionDto
    {
        public Guid QuestionId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? Exploration { get; set; }
        public List<SaveAnswerDto> Answers { get; set; }
    }

    public class SaveAnswerDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
    }
}

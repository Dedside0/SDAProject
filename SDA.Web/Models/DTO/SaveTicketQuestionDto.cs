namespace SDA.Web.Models.DTO
{
    public class SaveTicketQuestionDto
    {
        public Guid QuestionId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? Topic { get; set; }
        public string? Exploration { get; set; }
        public List<SaveAnswerDto> Answers { get; set; }
    }
}

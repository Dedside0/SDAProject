namespace SDA.Web.Models.DTO
{
    
    public class SaveTicketDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Theme { get; set; }
        public string? Description { get; set; }
        public List<SaveTicketQuestionDto> TicketQuestions { get; set; }
    }
}

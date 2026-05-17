namespace SDA.Web.Models.DTO
{
    public class CheckExamDTO
    {
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
        public List<CheckAnswerDTO> CheckAnswers { get; set; }
    }
}

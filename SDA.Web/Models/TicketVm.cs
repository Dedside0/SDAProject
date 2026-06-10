namespace SDA.Web.Models
{
    public class TicketVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<TicketQuestionVm> TicketQuestions { get; set; }
    }
}

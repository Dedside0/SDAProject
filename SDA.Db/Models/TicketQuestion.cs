namespace SDA.Db.Models
{
    public class TicketQuestion
    {
        public Guid QuestionId { get; set; }
        public Guid TicketId { get; set; }

        public Question Question { get; set; }
        public Ticket Ticket { get; set; }

        public int Order {  get; set; }
    }
}

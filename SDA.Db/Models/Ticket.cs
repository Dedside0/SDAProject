namespace SDA.Db.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<TicketQuestion> TicketQuestions { get; set; }

    }
}

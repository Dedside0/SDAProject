namespace SDA.Db.Models
{
    public class Question
    {
        public Guid Id { get; set; }

        public string? Explanation { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }

        public List<Answer> Answers { get; set; } = [];
        public int? TopicId { get; set; }
        public Topic? Topic { get; set; }
    }
}

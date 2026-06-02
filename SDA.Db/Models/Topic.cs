namespace SDA.Db.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Group { get; set; }
        public List<Question> Questions { get; set; } = [];
    }
}

namespace SDA.Db.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public int QuestionThemeId { get; set; }
        public QuestionTheme QuestionTheme { get; set; }

        public List<Answer> Answers { get; set; }
        public string Exploration { get; set; }
        public string? Theme { get; set; }
        public string? Topic { get; set; }

        public string Text { get; set; }
        public string? ImageUrl { get; set; }
    }
}

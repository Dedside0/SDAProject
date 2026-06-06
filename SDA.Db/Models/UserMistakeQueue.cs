namespace SDA.Db.Models
{

    public class UserMistakeQueue
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime AddedAt { get; set; }    
        public bool IsMastered { get; set; }

        // Навигационные свойства
        public User? User { get; set; }
        public Question Question { get; set; }
    }
}
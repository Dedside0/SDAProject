namespace SDA.Db.Models
{

    public class UserMistakeQueue
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime AddedAt { get; set; }    // Когда добавили в очередь
        public bool IsMastered { get; set; }      // Освоен ли вопрос

        // Навигационные свойства
        public User? User { get; set; }
        public Question Question { get; set; }
    }
}
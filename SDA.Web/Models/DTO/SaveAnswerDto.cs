namespace SDA.Web.Models.DTO
{
    public class SaveAnswerDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
        public int Order { get; set; }
    }
}

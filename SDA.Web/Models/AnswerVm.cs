namespace SDA.Web.Models
{
    public class AnswerVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsRight { get; set; }
    }
}

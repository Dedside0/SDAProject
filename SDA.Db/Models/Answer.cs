using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Models
{
    public class Answer
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public string Text { get; set; }
        public bool IsRight { get; set; }
    }
}

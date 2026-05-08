using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public List<TicketQuestion> TicketQuestions { get; set; }




    }
}

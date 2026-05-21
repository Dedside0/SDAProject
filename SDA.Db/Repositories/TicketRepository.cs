using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    internal class TicketRepository(AppContext ddd) : ITicketRepository
    {
        public async Task<List<Ticket>?> GetAll() => ddd.Tickets
            .AsNoTracking()
            .Include(x => x.TicketQuestions)
                .ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Answers)
            .AsSplitQuery()
            .ToList();

        public async Task<Ticket?> GetById(Guid id) =>
             await ddd.Tickets
                .AsNoTracking()
                .Include(x => x.TicketQuestions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Answers)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task Create(Ticket ticket)
        {
            if (await GetById(ticket.Id) is not null)
            {
                await Update(ticket);
                return;
            }

            await ddd.AddAsync(ticket);
            await ddd.SaveChangesAsync();
        }

        public async Task Update(Ticket ticket)
        {
            var existingTicket = await GetById(ticket.Id);
            if (existingTicket == null)
            {
                await Create(ticket);
                return;
            }

            existingTicket.Name = ticket.Name;


            existingTicket.TicketQuestions.Clear();
            foreach (var question in ticket.TicketQuestions)
            {
                existingTicket.TicketQuestions.Add(question);
            }

            await ddd.SaveChangesAsync();
        }
    }
}

using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    internal interface ITicketRepository
    {
        Task Create(Ticket ticket);
        List<Ticket>? GetAll();
        Task<Ticket?> GetById(Guid id);
        Task Update(Ticket ticket);
    }
}
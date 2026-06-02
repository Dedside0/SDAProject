using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface ITicketRepository
    {
        Task Create(Ticket ticket);
        Task<List<Ticket>?> GetAll();
        Task<Ticket?> GetById(Guid id, bool track = false);
        Task Update(Ticket ticket);
    }
}
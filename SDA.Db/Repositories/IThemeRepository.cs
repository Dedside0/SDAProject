using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IThemeRepository
    {
        Task Create(Topic question);
        Task<List<Topic>?> GetAll();
        Task<Topic?> GetById(int id);
        Task Update(Topic question);
        Task Delete(int id);
    }
}
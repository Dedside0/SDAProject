using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IThemeRepository
    {
        Task Create(QuestionTheme question);
        Task<List<QuestionTheme>?> GetAll();
        Task<QuestionTheme?> GetById(int id);
        Task Update(QuestionTheme question);
        Task Delete(int id);
    }
}
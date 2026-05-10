using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IQuestionRepository
    {
        Task Create(Question question);
        List<Question>? GetAll();
        Task<Question?> GetById(Guid id);
        Task Update(Question question);
    }
}
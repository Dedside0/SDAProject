using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    internal interface IQuestionRepository
    {
        Task Create(Question question);
        List<Question>? GetAll();
        Task<Question?> GetById(Guid id);
        Task Update(Question question);
    }
}
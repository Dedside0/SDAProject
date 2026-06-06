using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IExamAttemptRepository
    {
        void Create(ExamAttempt attempt);
        ExamAttempt? GetById(Guid id);
        List<ExamAttempt>? GetAllByUserId(Guid userId);
    }
}
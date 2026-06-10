using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IQuestionAttemptRepository
    {
        void Create(QuestionAttempt questionAttempt);
        List<QuestionAttempt> GetListById(Guid attemptId);

        List<QuestionAttempt> GetUserAttempts(Guid userId);
    }
}
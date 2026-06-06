using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public interface IUserMistakeQueueRepository
    {
        void Create(UserMistakeQueue userMistakeQueue);
        void Delete(Guid id);
        UserMistakeQueue? GetById(Guid id);
        List<UserMistakeQueue>? GetAllByUserId(Guid id);
        UserMistakeQueue? GetUserMistake(Guid userId, Guid questionId);
        void SetAsMastered(Guid id);
    }
}

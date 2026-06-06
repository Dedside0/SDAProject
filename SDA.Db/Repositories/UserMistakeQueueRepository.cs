using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    public class UserMistakeQueueRepository(AppContext dbContext): IUserMistakeQueueRepository
    {
        public void Create(UserMistakeQueue userMistakeQueue)
        {
            var existing = dbContext.MistakeQueue.FirstOrDefault(x => x.Id == userMistakeQueue.Id);
            if (existing is not null)
                throw new DuplicateWaitObjectException("Уже существует");
            dbContext.MistakeQueue.Add(userMistakeQueue);
            dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = dbContext.MistakeQueue.FirstOrDefault(x => x.Id == id);
            if (existing is  null)
                throw new KeyNotFoundException("Не существует");

            dbContext.MistakeQueue.Remove(existing);
            dbContext.SaveChanges();
        }

        public UserMistakeQueue? GetById(Guid id)
        {
            var existing = dbContext.MistakeQueue.AsNoTracking().FirstOrDefault(x => x.Id == id && x.IsMastered == false);

            return existing;
        }

        public List<UserMistakeQueue>? GetAllByUserId(Guid id)
        {
            var existing = dbContext.MistakeQueue.AsNoTracking().Where(x => x.UserId == id && x.IsMastered == false).ToList();
            return existing;
        }

        public  UserMistakeQueue? GetUserMistake(Guid userId, Guid questionId)
        {
            return dbContext.MistakeQueue
                    .FirstOrDefault(m => m.UserId == userId
                        && m.QuestionId == questionId
                        && !m.IsMastered);
        }

        public void SetAsMastered(Guid id)
        {
            var mstk = dbContext.MistakeQueue.Find(id);
            mstk.IsMastered = true;
            dbContext.SaveChanges();
        }

    }
}

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
            var existing = dbContext.MistakeQueue.AsNoTracking().FirstOrDefault(x => x.Id == id);

            return existing;
        }

        public List<UserMistakeQueue>? GetAllByUserId(Guid id)
        {
            var existing = dbContext.MistakeQueue.AsNoTracking().Where(x => x.UserId == id).ToList();
            return existing;
        }

    }
}

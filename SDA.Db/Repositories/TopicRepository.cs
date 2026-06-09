using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;

namespace SDA.Db.Repositories
{
    internal class TopicRepository(AppContext dbContext): ITopicRepository
    {
        public async Task<List<Topic>?> GetAll() =>
           await dbContext.Topics.AsNoTracking()
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers).AsSplitQuery()
                .ToListAsync();

        public async Task<Topic?> GetById(int id) => await dbContext.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        public async Task<Topic?> Create(Topic topic)
        {
            var existing = await GetById(topic.Id);
            if(existing is not null)
                throw new DuplicateWaitObjectException($"Тема с таким Id уже существует");

            await dbContext.Topics.AddAsync(topic);
            await dbContext.SaveChangesAsync(); 

            return topic; 
        }

        public async Task Update(Topic theme)
        {
            var existing = await dbContext.Topics.FirstOrDefaultAsync(x => x.Id == theme.Id);
            if (existing is null)
                throw new KeyNotFoundException($"Тема с Id не найдена.");

            existing.Name=theme.Name;
            existing.Group=theme.Group;
            await dbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var existing = await dbContext.Topics.FirstOrDefaultAsync(x => x.Id == id);
            if(existing is null)
                throw new KeyNotFoundException($"Тема с Id не найдена.");

            dbContext.Topics.Remove(existing);
            await dbContext.SaveChangesAsync() ;
        }

        public async Task<Topic?> GetByName(string name) => await dbContext.Topics
            .AsNoTracking().
            FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

        public async Task<List<Topic>> GetQuestionsGrouped()
        {
            return await dbContext.Topics
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .ToListAsync();
        }
    }
}

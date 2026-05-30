using Microsoft.EntityFrameworkCore;
using SDA.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SDA.Db.Repositories
{
    internal class ThemeRepository(AppContext dbContext): IThemeRepository
    {
        public async Task<List<Topic>?> GetAll() =>
           await dbContext.Themes
           .AsNoTracking()
           .ToListAsync();

        public async Task<Topic?> GetById(int id) => await dbContext.Themes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        public async Task Create(Topic theme)
        {
            var existing = await GetById(theme.Id);
            if(existing is not null)
                throw new DuplicateWaitObjectException($"Тема с таким Id уже существует");

            await dbContext.Themes.AddAsync(theme);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Topic theme)
        {
            var existing = await dbContext.Themes.FirstOrDefaultAsync(x => x.Id == theme.Id);
            if (existing is null)
                throw new KeyNotFoundException($"Тема с Id не найдена.");

            existing.Name=theme.Name;
            await dbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var existing = await dbContext.Themes.FirstOrDefaultAsync(x => x.Id == id);
            if(existing is null)
                throw new KeyNotFoundException($"Тема с Id не найдена.");

            dbContext.Themes.Remove(existing);
            await dbContext.SaveChangesAsync() ;
        }
    }
}

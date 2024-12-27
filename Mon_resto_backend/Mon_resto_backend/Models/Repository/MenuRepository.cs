using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Models.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly MonRestoDbContext _context;

        public MenuRepository(MonRestoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> GetMenusAsync() => await _context.Menus
            .Include(m => m.MenuArticles)
            .ThenInclude(ma => ma.Article)
            .ToListAsync();

        public async Task<Menu> GetMenuByIdAsync(int id) => await _context.Menus
            .Include(m => m.MenuArticles)
            .ThenInclude(ma => ma.Article)
            .FirstOrDefaultAsync(m => m.Id == id);

        public async Task AddMenuAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
    }
}

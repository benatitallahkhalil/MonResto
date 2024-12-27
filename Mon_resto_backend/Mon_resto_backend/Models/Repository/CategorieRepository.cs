using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mon_resto_backend.Models.Repository
{
    public class CategorieRepository : ICategorieRepository
    {
        private readonly MonRestoDbContext _context;

        public CategorieRepository(MonRestoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorie>> GetCategoriesAsync()
        {
            return await _context.Categories.Include(c => c.Articles).ToListAsync();
        }

        public async Task<Categorie> GetCategorieByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Articles).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCategorieAsync(Categorie categorie)
        {
            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategorieAsync(Categorie categorie)
        {
            _context.Categories.Update(categorie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategorieAsync(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
            }
        }
    }
}

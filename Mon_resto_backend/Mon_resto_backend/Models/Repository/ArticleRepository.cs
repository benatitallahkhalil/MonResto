using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Models.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MonRestoDbContext _context;

        public ArticleRepository(MonRestoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            // Inclure les catégories
            return await _context.Articles.Include(a => a.Categorie).ToListAsync();
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            // Inclure la catégorie
            return await _context.Articles.Include(a => a.Categorie).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddArticleAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}

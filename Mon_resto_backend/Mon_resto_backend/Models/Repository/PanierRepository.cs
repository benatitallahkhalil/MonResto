using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Models.Repository
{
    public class PanierRepository : IPanierRepository
    {
        private readonly MonRestoDbContext _context;

        public PanierRepository(MonRestoDbContext context)
        {
            _context = context;
        }

        public async Task<Panier> GetPanierAsync(int panierId)
        {
            return await _context.Paniers
                .Include(p => p.Items)
                .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(p => p.Id == panierId);
        }

        public async Task<Panier> AjouterArticleAsync(int panierId, int articleId, int quantite)
        {
            var panier = await GetPanierAsync(panierId) ?? new Panier();
            var article = await _context.Articles.FindAsync(articleId);

            if (article == null)
            {
                throw new ArgumentException("L'article spécifié est introuvable.");
            }

            var item = panier.Items.FirstOrDefault(i => i.ArticleId == articleId);

            if (item == null)
            {
                panier.Items.Add(new PanierItem
                {
                    ArticleId = articleId,
                    Quantite = quantite,
                    PrixUnitaire = article.Prix
                });
            }
            else
            {
                item.Quantite += quantite;
            }

            if (panier.Id == 0)
            {
                _context.Paniers.Add(panier);
            }

            await _context.SaveChangesAsync();
            return panier;
        }

        public async Task<Panier> SupprimerArticleAsync(int panierId, int articleId)
        {
            var panier = await GetPanierAsync(panierId);

            if (panier == null)
            {
                throw new ArgumentException("Le panier spécifié est introuvable.");
            }

            var item = panier.Items.FirstOrDefault(i => i.ArticleId == articleId);

            if (item != null)
            {
                panier.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return panier;
        }

        public async Task<Panier> ModifierQuantiteAsync(int panierId, int articleId, int nouvelleQuantite)
        {
            var panier = await _context.Paniers
                .Include(p => p.Items)
                .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(p => p.Id == panierId);

            if (panier == null)
            {
                throw new ArgumentException("Le panier est introuvable.");
            }

            var item = panier.Items.FirstOrDefault(i => i.ArticleId == articleId);
            if (item == null)
            {
                throw new ArgumentException("L'article n'est pas dans le panier.");
            }

            if (nouvelleQuantite <= 0)
            {
                // Si la quantité est inférieure ou égale à 0, supprimez l'article.
                panier.Items.Remove(item);
            }
            else
            {
                // Sinon, mettez à jour la quantité.
                item.Quantite = nouvelleQuantite;
            }

            await _context.SaveChangesAsync();
            return panier;
        }
        public async Task<Panier> ViderPanierAsync(int panierId)
        {
            var panier = await GetPanierAsync(panierId);

            if (panier == null)
            {
                throw new ArgumentException("Le panier est introuvable.");
            }

            panier.Items.Clear(); // Supprime tous les éléments du panier.

            await _context.SaveChangesAsync();
            return panier;
        }

    }
}

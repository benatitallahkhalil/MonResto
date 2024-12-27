using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Models.Repository
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly MonRestoDbContext _context;

        public CommandeRepository(MonRestoDbContext context)
        {
            _context = context;
        }

        public async Task<Commande> CreerCommandeDepuisPanierAsync(int panierId, int userId)
        {
            var panier = await _context.Paniers
                .Include(p => p.Items)
                .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(p => p.Id == panierId);

            if (panier == null)
            {
                throw new ArgumentException("Panier introuvable.");
            }

            var commande = new Commande
            {
                UserId = userId,
                PrixTotal = panier.CalculerTotal(),
                DateCommande = DateTime.Now,
            };

            foreach (var item in panier.Items)
            {
                commande.Items.Add(new CommandeItem
                {
                    ArticleId = item.ArticleId,
                    Quantite = item.Quantite,
                    PrixUnitaire = item.PrixUnitaire
                });
            }

            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            // Vider le panier après la création de la commande
            panier.Items.Clear();
            await _context.SaveChangesAsync();

            return commande;
        }

        public async Task<Commande> AnnulerCommandeAsync(int commandeId)
        {
            var commande = await _context.Commandes
                .FirstOrDefaultAsync(c => c.Id == commandeId);

            if (commande == null)
            {
                throw new ArgumentException("Commande introuvable.");
            }

            commande.EstAnnulee = true;
            await _context.SaveChangesAsync();

            return commande;
        }

        public async Task<Commande> GetCommandeAsync(int commandeId)
        {
            return await _context.Commandes
                .Include(c => c.Items)
                .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(c => c.Id == commandeId);
        }

        // Nouvelle méthode pour récupérer les commandes d'un utilisateur
        public async Task<IEnumerable<Commande>> GetCommandesByUserIdAsync(int userId)
        {
            return await _context.Commandes
                .Where(c => c.UserId == userId)
                .Include(c => c.Items)
                .ThenInclude(i => i.Article)
                .ToListAsync();
        }
    }
}

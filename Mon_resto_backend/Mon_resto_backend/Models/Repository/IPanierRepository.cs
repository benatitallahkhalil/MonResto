namespace Mon_resto_backend.Models.Repository
{
    public interface IPanierRepository
    {
        Task<Panier> GetPanierAsync(int panierId);
        Task<Panier> AjouterArticleAsync(int panierId, int articleId, int quantite);
        Task<Panier> SupprimerArticleAsync(int panierId, int articleId);
        Task<Panier> ModifierQuantiteAsync(int panierId, int articleId, int nouvelleQuantite); // Nouvelle méthode pour modifier la quantité d'un article dans le panier
        Task<Panier> ViderPanierAsync(int panierId);

    }
}

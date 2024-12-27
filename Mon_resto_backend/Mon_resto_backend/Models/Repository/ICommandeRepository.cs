namespace Mon_resto_backend.Models.Repository
{
    public interface ICommandeRepository
    {
        Task<Commande> CreerCommandeDepuisPanierAsync(int panierId, int userId);
        Task<Commande> AnnulerCommandeAsync(int commandeId);
        Task<Commande> GetCommandeAsync(int commandeId);
        Task<IEnumerable<Commande>> GetCommandesByUserIdAsync(int userId); // Nouvelle méthode

    }
}
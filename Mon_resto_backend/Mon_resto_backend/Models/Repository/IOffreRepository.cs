namespace Mon_resto_backend.Models.Repository
{
    public interface IOffreRepository
    {
        Task<Offre> CreerOffreAsync(Offre offre);
        Task<Offre?> ObtenirOffreParIdAsync(int id);
        Task<List<Offre>> ObtenirToutesLesOffresAsync();
        Task SupprimerOffreAsync(int id);
        Task<List<Offre>> ObtenirOffresActivesAsync();

    }

}

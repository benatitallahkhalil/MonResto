namespace Mon_resto_backend.Models.Repository
{
    public interface ICategorieRepository
    {
        Task<IEnumerable<Categorie>> GetCategoriesAsync();
        Task<Categorie> GetCategorieByIdAsync(int id);
        Task AddCategorieAsync(Categorie categorie);
        Task UpdateCategorieAsync(Categorie categorie);
        Task DeleteCategorieAsync(int id);
    }
}

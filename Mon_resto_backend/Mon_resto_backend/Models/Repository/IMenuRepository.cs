namespace Mon_resto_backend.Models.Repository
{
    public interface IMenuRepository
    {

      
        Task<IEnumerable<Menu>> GetMenusAsync();
        Task<Menu> GetMenuByIdAsync(int id);
        Task AddMenuAsync(Menu menu);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int id);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Models.Repository;
using Mon_resto_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuRepository _repository;
        private readonly MonRestoDbContext _context;

        public MenusController(IMenuRepository repository, MonRestoDbContext context)
        {
            _repository = repository;
            _context = context;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetMenus() => Ok(await _repository.GetMenusAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var menu = await _repository.GetMenuByIdAsync(id);
            return menu == null ? NotFound() : Ok(menu);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(Menu menu)
        {
            // Vérifier que tous les articles associés existent
            foreach (var menuArticle in menu.MenuArticles)
            {
                var article = await _context.Articles.FindAsync(menuArticle.ArticleId);
                if (article == null)
                {
                    return BadRequest($"L'article avec l'ID {menuArticle.ArticleId} n'a pas été trouvé.");
                }

                // Associer l'article au menu via MenuArticle
                menuArticle.Article = article; // Associer l'entité Article à MenuArticle
            }

            // Ajouter le menu à la base de données
            await _repository.AddMenuAsync(menu);

            return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, Menu menu)
        {
            if (id != menu.Id) return BadRequest();
            await _repository.UpdateMenuAsync(menu);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            await _repository.DeleteMenuAsync(id);
            return NoContent();
        }
    }
}

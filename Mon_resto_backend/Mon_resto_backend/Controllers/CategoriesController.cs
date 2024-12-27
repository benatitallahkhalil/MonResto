using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Models;
using Mon_resto_backend.Models.Repository;
using System.Threading.Tasks;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategorieRepository _repository;

        public CategoriesController(ICategorieRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _repository.GetCategoriesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategorieById(int id)
        {
            var categorie = await _repository.GetCategorieByIdAsync(id);
            return categorie == null ? NotFound() : Ok(categorie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategorie(Categorie categorie)
        {
            await _repository.AddCategorieAsync(categorie);
            return CreatedAtAction(nameof(GetCategorieById), new { id = categorie.Id }, categorie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategorie(int id, Categorie categorie)
        {
            if (id != categorie.Id) return BadRequest();
            await _repository.UpdateCategorieAsync(categorie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            await _repository.DeleteCategorieAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Models.Repository;
using Mon_resto_backend.Models;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _repository;

        public ArticlesController(IArticleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _repository.GetArticlesAsync();

            // Transformation pour inclure les catégories
            var response = articles.Select(a => new
            {
                a.Id,
                a.Nom,
                a.Description,
                a.Prix,
                a.ImageUrl,
                Categorie = a.Categorie != null ? new
                {
                    a.Categorie.Id,
                    a.Categorie.Nom,
                    a.Categorie.Description
                } : null
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = await _repository.GetArticleByIdAsync(id);

            if (article == null)
                return NotFound();

            // Transformation pour inclure la catégorie
            var response = new
            {
                article.Id,
                article.Nom,
                article.Description,
                article.Prix,
                article.ImageUrl,
                Categorie = article.Categorie != null ? new
                {
                    article.Categorie.Id,
                    article.Categorie.Nom,
                    article.Categorie.Description
                } : null
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            await _repository.AddArticleAsync(article);
            return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, Article article)
        {
            if (id != article.Id)
                return BadRequest();

            await _repository.UpdateArticleAsync(article);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _repository.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Models.Repository;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaniersController : ControllerBase
    {
        private readonly IPanierRepository _repository;

        public PaniersController(IPanierRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPanier(int id)
        {
            var panier = await _repository.GetPanierAsync(id);
            if (panier == null)
            {
                return NotFound("Panier introuvable.");
            }

            return Ok(panier);
        }

        [HttpPost("{id}/articles/{articleId}")]
        public async Task<IActionResult> AjouterArticle(int id, int articleId, [FromQuery] int quantite)
        {
            var panier = await _repository.AjouterArticleAsync(id, articleId, quantite);
            return Ok(panier);
        }

        [HttpDelete("{id}/articles/{articleId}")]
        public async Task<IActionResult> SupprimerArticle(int id, int articleId)
        {
            var panier = await _repository.SupprimerArticleAsync(id, articleId);
            return Ok(panier);
        }

        [HttpGet("{id}/total")]
        public async Task<IActionResult> CalculerTotal(int id)
        {
            var panier = await _repository.GetPanierAsync(id);
            if (panier == null)
            {
                return NotFound("Panier introuvable.");
            }

            var total = panier.CalculerTotal();
            return Ok(new { Total = total });
        }

        [HttpPut("{panierId}/modifier-quantite")]
        public async Task<IActionResult> ModifierQuantite(int panierId, int articleId, int nouvelleQuantite)
        {
            try
            {
                var panier = await _repository.ModifierQuantiteAsync(panierId, articleId, nouvelleQuantite);
                return Ok(panier);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}/vider")]
        public async Task<IActionResult> ViderPanier(int id)
        {
            try
            {
                var panier = await _repository.ViderPanierAsync(id);
                return Ok(panier);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}

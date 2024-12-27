using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Models.Repository;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly ICommandeRepository _repository;

        public CommandesController(ICommandeRepository repository)
        {
            _repository = repository;
        }

        // Créer une commande depuis un panier
        [HttpPost("creerDepuisPanier/{panierId}")]
        public async Task<IActionResult> CreerCommandeDepuisPanier(int panierId, [FromQuery] int userId)
        {
            try
            {
                var commande = await _repository.CreerCommandeDepuisPanierAsync(panierId, userId);
                return CreatedAtAction(nameof(GetCommande), new { id = commande.Id }, commande);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Annuler une commande
        [HttpPut("{commandeId}/annuler")]
        public async Task<IActionResult> AnnulerCommande(int commandeId)
        {
            try
            {
                var commande = await _repository.AnnulerCommandeAsync(commandeId);
                return Ok(commande);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Récupérer une commande
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommande(int id)
        {
            var commande = await _repository.GetCommandeAsync(id);
            if (commande == null)
            {
                return NotFound("Commande introuvable.");
            }

            return Ok(commande);
        }

        // Nouvelle action pour récupérer les commandes d'un utilisateur
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommandesByUserId(int userId)
        {
            var commandes = await _repository.GetCommandesByUserIdAsync(userId);
            if (commandes == null || !commandes.Any())
            {
                return NotFound("Aucune commande trouvée pour cet utilisateur.");
            }

            return Ok(commandes);
        }
    }
}

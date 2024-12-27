using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mon_resto_backend.Dtos;
using Mon_resto_backend.Models.Repository;
using Mon_resto_backend.Models;

namespace Mon_resto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffresController : ControllerBase
    {
        private readonly IOffreRepository _offreRepository;

        public OffresController(IOffreRepository offreRepository)
        {
            _offreRepository = offreRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreerOffre([FromBody] OffreDto offreDto)
        {
            var nouvelleOffre = new Offre
            {
                Titre = offreDto.Titre,
                Description = offreDto.Description,
                Remise = offreDto.Remise,
                DateDebut = offreDto.DateDebut,
                DateFin = offreDto.DateFin,
                ArticleId = offreDto.ArticleId
            };

            var offreCreee = await _offreRepository.CreerOffreAsync(nouvelleOffre);
            return Ok(offreCreee);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenirOffre(int id)
        {
            var offre = await _offreRepository.ObtenirOffreParIdAsync(id);
            if (offre == null)
            {
                return NotFound("Offre introuvable.");
            }

            return Ok(offre);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenirToutesLesOffres()
        {
            var offres = await _offreRepository.ObtenirToutesLesOffresAsync();
            return Ok(offres);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SupprimerOffre(int id)
        {
            await _offreRepository.SupprimerOffreAsync(id);
            return Ok("Offre supprimée avec succès.");
        }
        [HttpGet("actives")]
        public async Task<IActionResult> ObtenirOffresActives()
        {
            var offresActives = await _offreRepository.ObtenirOffresActivesAsync();
            return Ok(offresActives);
        }

    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mon_resto_backend.Dtos;
using Mon_resto_backend.Models;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProfilController : ControllerBase
{
    private readonly MonRestoDbContext _context;

    public ProfilController(MonRestoDbContext context)
    {
        _context = context;
    }

    // Récupérer le profil de l'utilisateur connecté
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("Utilisateur introuvable.");

        return Ok(new UserProfileDto
        {
            Nom = user.Nom,
            Email = user.Email
        });
    }

    // Mettre à jour le profil de l'utilisateur connecté
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("Utilisateur introuvable.");

        // Vérifier si l'email est déjà utilisé
        if (_context.Users.Any(u => u.Email == dto.Email && u.Id != userId))
        {
            return BadRequest("L'email est déjà pris.");
        }

        // Mettre à jour les données du profil
        user.Nom = dto.Nom;
        user.Email = dto.Email;

        await _context.SaveChangesAsync();
        return Ok("Profil mis à jour avec succès.");
    }

    // Récupérer l'historique des commandes de l'utilisateur connecté
    [HttpGet("me/commandes")]
    public async Task<IActionResult> GetOrderHistory()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var commandes = await _context.Commandes
            .Where(c => c.UserId == userId)
            .Include(c => c.Items)
            .ThenInclude(i => i.Article)
            .ToListAsync();

        if (!commandes.Any())
        {
            return NotFound("Aucune commande trouvée pour cet utilisateur.");
        }

        var result = commandes.Select(c => new
        {
            CommandeId = c.Id,
            DateCommande = c.DateCommande,
            PrixTotal = c.PrixTotal,
            EstAnnulee = c.EstAnnulee,
            Items = c.Items.Select(i => new
            {
                ArticleId = i.ArticleId,
                ArticleName = i.Article?.Nom,
                Quantite = i.Quantite,
                PrixUnitaire = i.PrixUnitaire
            })
        });

        return Ok(result);
    }
}

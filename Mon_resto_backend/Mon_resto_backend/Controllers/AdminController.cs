using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mon_resto_backend.Models;
using Mon_resto_backend.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Mon_resto_backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MonRestoDbContext _context;

        public AdminController(MonRestoDbContext context)
        {
            _context = context;
        }

        // Voir tous les utilisateurs
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.Select(u => new
            {
                u.Id,
                u.Nom,
                u.Email,
                u.Role
            }).ToListAsync();

            return Ok(users);
        }

        // Modifier un utilisateur
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("Utilisateur introuvable.");

            user.Nom = dto.Nom;
            user.Email = dto.Email;
            user.Role = dto.Role;

            await _context.SaveChangesAsync();
            return Ok("Utilisateur mis à jour avec succès.");
        }

        // Supprimer un utilisateur
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("Utilisateur introuvable.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur supprimé avec succès.");
        }
    }
}

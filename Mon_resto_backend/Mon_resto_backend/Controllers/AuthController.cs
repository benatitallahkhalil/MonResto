using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MonResto_backend.Services;

using System.Linq;
using System;
using Mon_resto_backend.Dtos;
using Mon_resto_backend.Models;

namespace MonResto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly MonRestoDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;  // Déclaration du hachage

        public AuthController(JwtService jwtService, IConfiguration configuration, MonRestoDbContext context)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();  // Initialisation du hachage
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == registerDto.Email))
                {
                    return BadRequest("L'utilisateur existe déjà.");
                }

                // Création de l'utilisateur
                var user = new User
                {
                    Email = registerDto.Email,
                    Nom = registerDto.Nom,  // Assurez-vous que le champ "Nom" existe dans le modèle User
                    Role = "User" // Exemple de rôle, à ajuster selon votre logique
                };

                // Hachage du mot de passe avant de l'enregistrer
                user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

                _context.Users.Add(user);
                _context.SaveChanges();  // Cette ligne pourrait générer l'erreur

                return Ok(new { message = "Inscription réussie." });
            }
            catch (Exception ex)
            {
                // Log l'exception pour déboguer
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Une erreur est survenue lors de l'inscription : " + ex.Message);
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    return Unauthorized("Identifiants invalides.");
                }

                // Vérification du mot de passe
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return Unauthorized("Identifiants invalides.");
                }

                // Générer le token JWT
                var token = _jwtService.GenerateToken(user);

                // Vérifiez et loggez les données utilisateur
                Console.WriteLine($"Utilisateur trouvé : Id={user.Id}, Nom={user.Nom}, Role={user.Role}");

                // Retourner le token et les informations utilisateur
                return Ok(new
                {
                    token,
                    userId = user.Id, // Vérifiez que cette propriété est bien définie et non nulle
                    nom = user.Nom,
                    email = user.Email,
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");
                return StatusCode(500, $"Une erreur est survenue lors de la connexion : {ex.Message}");
            }
        }
    }
}

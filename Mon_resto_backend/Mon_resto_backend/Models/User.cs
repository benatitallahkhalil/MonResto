namespace Mon_resto_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Stockage du hash du mot de passe
        public string Role { get; set; } = "User"; // Par défaut, un utilisateur est un client
        public ICollection<Commande> Commandes { get; set; }
    }
}

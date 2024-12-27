namespace Mon_resto_backend.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // ID de l'utilisateur
        public DateTime DateCommande { get; set; } = DateTime.Now;
        public decimal PrixTotal { get; set; }
        public bool EstAnnulee { get; set; } = false;
        public User? User { get; set; }


        public ICollection<CommandeItem> Items { get; set; } = new List<CommandeItem>();
    }

}

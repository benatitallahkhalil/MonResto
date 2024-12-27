using System.ComponentModel.DataAnnotations;

namespace Mon_resto_backend.Models
{
    public class Offre
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public decimal Remise { get; set; } // En pourcentage ou en valeur
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        public int ArticleId { get; set; }
        public Article ? Article { get; set; }
    }
}

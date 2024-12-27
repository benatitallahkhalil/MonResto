using System.ComponentModel.DataAnnotations;

namespace Mon_resto_backend.Dtos
{
    public class OffreDto
    {
        [Required]
        public string Titre { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 100, ErrorMessage = "La remise doit être entre 0 et 100.")]
        public decimal Remise { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }

}

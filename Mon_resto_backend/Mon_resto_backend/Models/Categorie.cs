using System.Text.Json.Serialization;

namespace Mon_resto_backend.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // Nouveau champ pour l'image
       
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}

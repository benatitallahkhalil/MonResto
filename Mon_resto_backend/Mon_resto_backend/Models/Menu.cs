using System.ComponentModel.DataAnnotations;

namespace Mon_resto_backend.Models
{
    public class Menu

    {
            [Key]
            public int Id { get; set; }
            public string Nom { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty; // Image associée au menu

            // Relation plusieurs-à-plusieurs avec les articles
            public ICollection<MenuArticle> MenuArticles { get; set; } = new List<MenuArticle>();
        

    }

}

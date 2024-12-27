using Mon_resto_backend.Models;

public class Article
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategorieId { get; set; }
    public Categorie? Categorie { get; set; } // Rendre nullable
    public ICollection<MenuArticle> MenuArticles { get; set; } = new List<MenuArticle>();
    public ICollection<Offre> Offres { get; set; } = new List<Offre>();
}

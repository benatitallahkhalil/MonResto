using Mon_resto_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class MenuArticle
{
    [Key]
    public int MenuId { get; set; }

    [JsonIgnore] // Ignorer la sérialisation de la propriété Menu
    public Menu ? Menu { get; set; }

    public int ArticleId { get; set; }

    [JsonIgnore] // Ignorer la sérialisation de la propriété Article
    public Article ? Article { get; set; }
}

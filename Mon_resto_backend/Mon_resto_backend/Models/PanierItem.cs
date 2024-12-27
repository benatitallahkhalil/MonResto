namespace Mon_resto_backend.Models
{
    public class PanierItem
    {
        public int Id { get; set; }
        public int PanierId { get; set; }
        public Panier Panier { get; set; }

        public int ArticleId { get; set; }
        public Article ? Article { get; set; }

        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal Total => Quantite * PrixUnitaire; //Ajoutez une méthode ou une logique permettant de mettre à jour la quantité d'un article.


    }
}

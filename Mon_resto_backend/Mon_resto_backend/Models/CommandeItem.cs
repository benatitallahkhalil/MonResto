namespace Mon_resto_backend.Models
{
    public class CommandeItem
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
namespace Mon_resto_backend.Models
{
    public class Panier
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public ICollection<PanierItem> Items { get; set; } = new List<PanierItem>();

        public decimal CalculerTotal()
        {
            return Items.Sum(item => item.PrixUnitaire * item.Quantite);
        }
    }
}

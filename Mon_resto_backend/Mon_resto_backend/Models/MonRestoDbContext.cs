using Microsoft.EntityFrameworkCore;

namespace Mon_resto_backend.Models
{
    public class MonRestoDbContext : DbContext
    {
        public MonRestoDbContext(DbContextOptions<MonRestoDbContext> options) : base(options) { }

        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<PanierItem> PanierItems { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandeItem> CommandeItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Offre> Offres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
         .HasOne(a => a.Categorie)
         .WithMany(c => c.Articles)
         .HasForeignKey(a => a.CategorieId)
         .OnDelete(DeleteBehavior.Cascade);  // This will enable cascading delete

            // Chargement automatique de la catégorie
            modelBuilder.Entity<Article>()
                .Navigation(a => a.Categorie)
                .AutoInclude();

            // Relation plusieurs-à-plusieurs Menu-Article
            modelBuilder.Entity<MenuArticle>()
                .HasKey(ma => new { ma.MenuId, ma.ArticleId });

            modelBuilder.Entity<MenuArticle>()
                .HasOne(ma => ma.Menu)
                .WithMany(m => m.MenuArticles)
                .HasForeignKey(ma => ma.MenuId);

            modelBuilder.Entity<MenuArticle>()
                .HasOne(ma => ma.Article)
                .WithMany(a => a.MenuArticles)
                .HasForeignKey(ma => ma.ArticleId);

            modelBuilder.Entity<PanierItem>()
                .HasOne(pi => pi.Panier)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PanierId);

            modelBuilder.Entity<PanierItem>()
                .HasOne(pi => pi.Article)
                .WithMany()
                .HasForeignKey(pi => pi.ArticleId);

            // Configuration de CommandeItem
            modelBuilder.Entity<CommandeItem>()
                .HasOne(ci => ci.Commande)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CommandeId);

            modelBuilder.Entity<CommandeItem>()
                .HasOne(ci => ci.Article)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            // Relations existantes
            modelBuilder.Entity<Categorie>()
                .HasMany(c => c.Articles)
                .WithOne(a => a.Categorie)
                .HasForeignKey(a => a.CategorieId) ;

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.User)
                .WithMany(u => u.Commandes)
                .HasForeignKey(c => c.UserId);


            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}

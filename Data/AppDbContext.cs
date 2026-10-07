using Microsoft.EntityFrameworkCore;

namespace RecipeApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne()
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Reviews)
                .WithOne()
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recipe>().Property(r => r.Servings).HasDefaultValue(1);

            modelBuilder.Entity<Ingredient>().Property(i => i.Quantity).HasPrecision(18, 2);
            modelBuilder.Entity<Ingredient>().Property(i => i.Calories).HasPrecision(18, 2);
            modelBuilder.Entity<Ingredient>().Property(i => i.Protein).HasPrecision(18, 2);
            modelBuilder.Entity<Ingredient>().Property(i => i.Carbs).HasPrecision(18, 2);
            modelBuilder.Entity<Ingredient>().Property(i => i.Fat).HasPrecision(18, 2);
        }
    }
}

using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Ingredient> Ingredients { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}
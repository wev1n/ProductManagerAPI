using ProductManagerAPI.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace ProductManagerAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}

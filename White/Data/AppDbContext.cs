using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace White.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }



    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    

}
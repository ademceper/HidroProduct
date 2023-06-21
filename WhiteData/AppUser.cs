using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WhiteData;

namespace WhiteData
{
    public class AppUser : IdentityUser<Guid>
    {
        public virtual required string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public virtual ICollection<Product>? CreatedProducts { get; set; } = new HashSet<Product>();
        public virtual ICollection<Category>? CreatedCategories { get; set; } = new HashSet<Category>();
    }
}
public class AppUserTypeConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder
            .HasIndex(a => a.UserName)
            .IsUnique();
        builder
            .Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(450);
        builder
            .HasMany(p => p.CreatedProducts)
            .WithOne(p => p.CreatorUser)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasMany(p => p.CreatedCategories)
            .WithOne(p => p.CreatorUser)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}

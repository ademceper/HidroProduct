using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using White.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace White.Data
{
    public class AppUser : IdentityUser<Guid>
    {
        public virtual required string Name { get; set; }

        public bool Enabled { get; set; } = true;

        public virtual string? SupplierName { get; set; }

        public virtual AppRole? AppRole { get; set; }
        public virtual ICollection<Product>? CreatedProducts { get; set; } = new HashSet<Product>();
        
        public string? Password { get; set; }
        [NotMapped]
        public bool IsAdministrator { get; set; } = false;
        [NotMapped]
        public bool IsGörüntüleyici { get; set; } = false;
        [NotMapped]
        public bool IsSilici { get; set; } = false;
        [NotMapped]
        public bool IsKaydedici { get; set; } = false;
        [NotMapped]
        public bool IsGüncelleyici { get; set; } = false;


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

       


    }
}

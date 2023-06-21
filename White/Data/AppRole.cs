using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace White.Data;


public class AppRole : IdentityRole<Guid>
{
    
    public virtual ICollection<AppUser>? AppUsers { get; set; } = new HashSet<AppUser>();


}

public class AppRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
       
    }
}
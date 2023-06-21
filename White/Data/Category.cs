using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using White.Data;

namespace White.Data
{
    public class Category
    {
        public Guid Id { get; set; }
        [Display(Name = "Kategori adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]

        public virtual required string Name { get; set; }
       
        public virtual Guid UserId { get; set; }
       
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasIndex(a => a.Name)
            .IsUnique();
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(450);
        builder
            .HasMany(p => p.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


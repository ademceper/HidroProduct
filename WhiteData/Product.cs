using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhiteData;

namespace WhiteData
{
    public class Product
    {
        public Guid id { get; set; }
        public virtual required string Name { get; set; }
        public int Piece { get; set; }
        public decimal Price { get; set; }
        public virtual string? Image { get; set; }
        public virtual required string SupplierName { get; set; }
        public virtual string? Description { get; set; }
        public Guid UserId { get; set; }
        public virtual AppUser? CreatorUser { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasIndex(p => p.Name)
            .IsUnique();
        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(450);
        builder
           .Property(p => p.Price)
           .HasPrecision(18, 4);
        builder
            .Property(p => p.Image)
            .IsUnicode(false);
    }
}

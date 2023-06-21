using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using White.Data;

namespace White.Data
{
    public class Product
    {
        public Guid id { get; set; }
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public virtual required string Name { get; set; }
        
        [Display(Name = "Adet")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int Piece { get; set; }
        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public decimal Price { get; set; }
        [Display(Name = "Resim")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public virtual required string Image { get; set; }
        [Display(Name = "Tedarikçi Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public virtual required string SupplierName { get; set; }
        [Display(Name = "Açıklama")]
        public virtual string? Description { get; set; }
        [Display(Name = "Oluşturucu")]
        public Guid UserId { get; set; }
        [Display(Name = "Oluşturucu")]
        public virtual AppUser? CreatorUser { get; set; }
        [Display(Name = "Kategori")]
        public Guid CategoryId { get; set; }
        [Display(Name = "Kategori")]
        public virtual Category? Category { get; set; }
      
        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression(@"[0-9]+(,[0-9]{2})?", ErrorMessage = "Lütfen geçerli bir fiyat yazınız!")]
        public required string PriceText { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

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
           .HasPrecision(18, 2);
        builder
            .Property(p => p.Image)
            .IsUnicode(false);
    }
}

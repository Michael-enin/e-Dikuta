using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataStore.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Price).IsRequired().HasConversion<double>();
            builder.HasOne(x => x.ProductBrand)
                   .WithMany()
                   .HasForeignKey(x => x.ProductBrandId);
            builder.HasOne(x => x.ProductType)
                   .WithMany()
                   .HasForeignKey(p => p.ProductTypeId);

        }
    }
}
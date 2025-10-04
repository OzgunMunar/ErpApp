using ERP.Domain.Entities;
using ERP.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.Property(p => p.ProductName).HasColumnType("varchar(50)");

            builder.Property(p => p.ProductType)
                .HasConversion(v => v.Value, v => ProductEnum.FromValue(v))
                .HasColumnName("ProductType");

        }
    }
}
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.Property(i => i.ProductName).HasColumnName("varchar(50)");

            builder.Property(p => p.ProductType)
                .HasConversion(v => v.Value, v => ProductEnum.FromValue(v))
                .HasColumnName("Product");

        }
    }
}

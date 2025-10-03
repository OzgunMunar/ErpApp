using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Configurations
{
    public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.Property(p => p.FullName).HasColumnType("varchar(50)");
            builder.Property(p => p.TaxDepartment).HasColumnType("varchar(50)");
            builder.Property(p => p.TaxDepartment).HasColumnType("varchar(50)");
            builder.Property(p => p.City).HasColumnType("varchar(50)");
            builder.Property(p => p.Town).HasColumnType("varchar(50)");
            builder.Property(p => p.Street).HasColumnType("varchar(50)");

        }
    }
}

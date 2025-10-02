using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Configurations
{
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
            builder.Property(p => p.LastName).HasColumnType("varchar(50)");
            builder.Property(p => p.IdentityNumber).HasColumnType("varchar(11)");
            builder.Property(i => i.Email).HasColumnName("Email");
            builder.Property(i => i.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(i => i.City).HasColumnName("City");
            builder.Property(i => i.Country).HasColumnName("Country");
            builder.Property(i => i.Street).HasColumnName("Street");

        }
    }
}

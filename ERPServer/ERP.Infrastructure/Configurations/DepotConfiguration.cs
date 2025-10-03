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
    public sealed class DepotConfiguration : IEntityTypeConfiguration<Depot>
    {
        public void Configure(EntityTypeBuilder<Depot> builder)
        {

            builder.Property(p => p.DepotName).HasColumnType("varchar(50)");
            builder.Property(p => p.City).HasColumnType("varchar(50)");
            builder.Property(p => p.Town).HasColumnType("varchar(50)");
            builder.Property(p => p.Street).HasColumnType("varchar(50)");

        }
    }
}

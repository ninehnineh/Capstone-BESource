using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Configuration
{
    public class ParkingConfiguration : IEntityTypeConfiguration<Domain.Entities.Parking>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Parking> builder)
        {
            builder.ToTable("Parking");

            builder.Property(e => e.Address).HasMaxLength(255);

            builder.Property(e => e.Code)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.Description).HasMaxLength(255);

            builder.Property(e => e.Latitude).HasColumnType("decimal(10, 6)");

            builder.Property(e => e.Longitude).HasColumnType("decimal(10, 6)");

            builder.Property(e => e.BusinessId).HasColumnName("BusinessId");

            builder.Property(e => e.Name).HasMaxLength(50);

            builder.HasOne(x => x.BusinessProfile)
                .WithMany(x => x.Parkings)
                .HasForeignKey(x => x.BusinessId)
                .HasConstraintName("FK__BusiessPro__Parking");
        }
    }
}

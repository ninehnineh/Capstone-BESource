using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Configuration
{
    public class FloorConfiguration : IEntityTypeConfiguration<Floor>
    {
        public void Configure(EntityTypeBuilder<Floor> builder)
        {
            builder.HasIndex(e => e.ParkingId, "IX_Floors_ParkingID");

            builder.Property(e => e.FloorName).HasMaxLength(50);

            builder.Property(e => e.ParkingId).HasColumnName("ParkingID");

            builder.HasOne(d => d.Parking)
                .WithMany(p => p.Floors)
                .HasForeignKey(d => d.ParkingId)
                .HasConstraintName("FK__Floors__ParkingI__47DBAE45");
        }
    }
}

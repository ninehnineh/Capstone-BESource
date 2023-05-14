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
    public class ParkingSpotImageConfiguration : IEntityTypeConfiguration<ParkingSpotImage>
    {
        public void Configure(EntityTypeBuilder<ParkingSpotImage> builder)
        {
            builder.ToTable("ParkingSpotImage");

            builder.HasIndex(e => e.ParkingId, "IX_ParkingSpotImage_ParkingID");

            builder.Property(e => e.ImgPath)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.ParkingId).HasColumnName("ParkingID");

            builder.HasOne(d => d.Parking)
                .WithMany(p => p.ParkingSpotImages)
                .HasForeignKey(d => d.ParkingId)
                .HasConstraintName("FK__ParkingSp__Parki__3C69FB99");
        }
    }
}

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
    public class ParkingSlotConfiguration : IEntityTypeConfiguration<ParkingSlot>
    {
        public void Configure(EntityTypeBuilder<ParkingSlot> builder)
        {

            builder.HasIndex(e => e.FloorId, "IX_ParkingSlots_FloorID");

            builder.HasIndex(e => e.TrafficId, "IX_ParkingSlots_TrafficID");

            builder.Property(e => e.FloorId).HasColumnName("FloorID");

            builder.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.TrafficId).HasColumnName("TrafficID");

            //builder.HasOne(d => d.Booking)
            //    .WithMany(p => p.ParkingSlots)
            //    .HasPrincipalKey(p => p.BookingId)
            //    .HasForeignKey(d => d.BookingId)
            //    .HasConstraintName("FK__ParkingSl__Booki__5629CD9C");

            builder.HasOne(d => d.Floor)
                .WithMany(p => p.ParkingSlots)
                .HasForeignKey(d => d.FloorId)
                .HasConstraintName("FK__ParkingSl__Floor__5441852A");


            builder.HasOne(d => d.Traffic)
                .WithMany(p => p.ParkingSlots)
                .HasForeignKey(d => d.TrafficId)
                .HasConstraintName("FK__ParkingSl__Traff");
        }
    }
}

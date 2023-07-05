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
    public class VehicleInforConfiguration : IEntityTypeConfiguration<VehicleInfor>
    {
        public void Configure(EntityTypeBuilder<VehicleInfor> builder)
        {
            builder.ToTable("Vehicle");

            builder.HasIndex(e => e.TrafficId, "IX_VehicleInfor_TrafficID");

            builder.HasIndex(e => e.UserId, "IX_VehicleInfor_UserID");

            builder.Property(e => e.Color).HasMaxLength(225);

            builder.Property(e => e.LicensePlate).HasMaxLength(225);

            builder.Property(e => e.TrafficId).HasColumnName("TrafficID");

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.Property(e => e.VehicleName).HasMaxLength(50);

            builder.HasOne(d => d.Traffic)
                .WithMany(p => p.VehicleInfors)
                .HasForeignKey(d => d.TrafficId)
                .HasConstraintName("FK__VehicleIn__Traff");

            builder.HasOne(d => d.User)
                .WithMany(p => p.VehicleInfors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__VehicleIn__UserI__4AB81AF0");
        }
    }
}

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
    public class StaffParkingConfiguration : IEntityTypeConfiguration<StaffParking>
    {
        public void Configure(EntityTypeBuilder<StaffParking> builder)
        {
            builder.ToTable("StaffParking");

            builder.HasIndex(e => e.ParkingId, "IX_StaffParking_ParkingID");

            builder.HasIndex(e => e.UserId, "IX_StaffParking_UserID");

            builder.Property(e => e.ParkingId).HasColumnName("ParkingID");

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.Parking)
                .WithMany(p => p.StaffParkings)
                .HasForeignKey(d => d.ParkingId)
                .HasConstraintName("FK__StaffPark__Parki__398D8EEE");

            builder.HasOne(d => d.User)
                .WithMany(p => p.StaffParkings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__StaffPark__UserI__38996AB5");
        }
    }
}

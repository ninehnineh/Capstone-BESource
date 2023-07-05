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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            //builder.HasKey(e => new { e.ParkingSlotId, e.StartTime, e.DateBook })
            //        .HasName("PK__Booking__1BDD09E6ABAB9F2E");

            builder.ToTable("Booking");

            //builder.HasKey(x => x.BookingId).HasName("PK_Booking");

            builder.HasIndex(e => e.UserId, "IX_Booking_UserID");

            builder.HasIndex(e => e.VehicleInforId, "IX_Booking_VehicleInforID");

            builder.Property(e => e.DateBook).HasColumnType("datetime2");

            builder.Property(e => e.StartTime).HasColumnType("datetime2");

            builder.Property(e => e.CheckinTime).HasColumnType("datetime2");

            builder.Property(e => e.CheckoutTime).HasColumnType("datetime2");

            builder.Property(e => e.EndTime).HasColumnType("datetime2");

            builder.Property(e => e.QRImage).HasMaxLength(255);

            builder.Property(e => e.BookingId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BookingID");

            builder.Property(e => e.GuestName).HasMaxLength(50);

            builder.Property(e => e.GuestPhone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();


            builder.Property(e => e.Status)
                .HasMaxLength(30);



            builder.Property(e => e.TotalPrice).HasColumnType("money");

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.Property(e => e.VehicleInforId).HasColumnName("VehicleInforID");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__UserID__5070F446");

            builder.HasOne(d => d.VehicleInfor)
                .WithMany(p => p.Bookings)
                .HasForeignKey(d => d.VehicleInforId)
                .HasConstraintName("FK__Booking__Vehicle");

        }
    }
}

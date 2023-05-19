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
            builder.HasKey(e => new { e.ParkingSlotId, e.StartTime, e.DateBook })
                    .HasName("PK__Booking__1BDD09E6ABAB9F2E");

            builder.ToTable("Booking");

            builder.HasIndex(e => e.BookingId, "AK_Booking_BookingIDsas")
                .IsUnique();

            builder.HasIndex(e => e.UserId, "IX_Booking_UserID");

            builder.HasIndex(e => e.VehicleInforId, "IX_Booking_VehicleInforID");

            builder.HasIndex(e => e.BookingId, "UQ__Booking__3214EC2628BBAE14")
                .IsUnique();

            builder.Property(e => e.ParkingSlotId).HasColumnName("ParkingSlotID");

            builder.Property(e => e.DateBook).HasColumnType("date");

            builder.Property(e => e.ActualPrice).HasColumnType("money");

            builder.Property(e => e.BookingId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BookingID");

            builder.Property(e => e.GuestName).HasMaxLength(50);

            builder.Property(e => e.GuestPhone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.PaymentMethod)
                .HasMaxLength(225)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .HasMaxLength(30);

            builder.Property(e => e.QrcodeText)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("QRCodeText");

            builder.Property(e => e.TmnCodeVnPay)
                .HasMaxLength(20)
                .IsUnicode(false);

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
                .HasConstraintName("FK__Booking__Vehicle__4F7CD00D");
        }
    }
}

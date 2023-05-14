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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasIndex(e => e.BookingId, "IX_Notifications_BookingID");

            builder.Property(e => e.Body).HasMaxLength(225);

            builder.Property(e => e.BookingId).HasColumnName("BookingID");

            builder.Property(e => e.Tiltle).HasMaxLength(225);

            builder.HasOne(d => d.Booking)
                .WithMany(p => p.Notifications)
                .HasPrincipalKey(p => p.BookingId)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Notificat__Booki__59063A47");
        }
    }
}

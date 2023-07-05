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
    public class BookingDetailsConfiguration : IEntityTypeConfiguration<BookingDetails>
    {
        public void Configure(EntityTypeBuilder<BookingDetails> builder)
        {
            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingDetails)
                .HasForeignKey(x => x.BookingId)
                .HasConstraintName("FK__Booking__BookingDetails");

            builder.HasOne(x => x.TimeSlot)
                .WithMany(x => x.BookingDetails)
                .HasForeignKey(x => x.TimeSlotId)
                .HasConstraintName("FK__TimeSlot__BookingDetails");

        }
    }
}

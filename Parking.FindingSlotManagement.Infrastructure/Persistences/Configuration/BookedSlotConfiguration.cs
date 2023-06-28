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
    public class BookedSlotConfiguration : IEntityTypeConfiguration<BookedSlot>
    {
        public void Configure(EntityTypeBuilder<BookedSlot> builder)
        {
            builder.HasOne(x => x.Parkingslot)
                .WithMany(x => x.BookedSlots)
                .HasForeignKey(x => x.ParkingSlotId)
                .HasConstraintName("FK_Parkingslot_BookedSlots");
        }
    }
}

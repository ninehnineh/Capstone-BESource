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
    public class BookedSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {

            builder.ToTable("TimeSlot");

            builder.HasOne(x => x.Parkingslot)
                .WithMany(x => x.TimeSlots)
                .HasForeignKey(x => x.ParkingSlotId)
                .HasConstraintName("FK_Parkingslot_BookedSlots");
        }
    }
}

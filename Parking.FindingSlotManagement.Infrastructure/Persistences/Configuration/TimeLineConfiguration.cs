using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Configuration
{
    public class TimeLineConfiguration : IEntityTypeConfiguration<TimeLine>
    {
        public void Configure(EntityTypeBuilder<TimeLine> builder)
        {
            builder.ToTable("TimeLine");

            builder.Property(e => e.Description).HasMaxLength(255);

            builder.Property(e => e.EndTime).HasColumnType("time");

            builder.Property(e => e.ExtraFee).HasColumnType("money");

            builder.Property(e => e.Name).HasMaxLength(50);

            builder.Property(e => e.Price).HasColumnType("money");

            builder.Property(e => e.StartTime).HasColumnType("time");

            builder.HasOne(d => d.ParkingPrice)
                .WithMany(p => p.TimeLines)
                .HasForeignKey(d => d.ParkingPriceId)
                .HasConstraintName("FK_Timeline_ParkingPrice");
        }
    }
}

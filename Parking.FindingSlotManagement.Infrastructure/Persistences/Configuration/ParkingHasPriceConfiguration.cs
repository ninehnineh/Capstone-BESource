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
    public class ParkingHasPriceConfiguration : IEntityTypeConfiguration<ParkingHasPrice>
    {
        public void Configure(EntityTypeBuilder<ParkingHasPrice> builder)
        {
            builder.ToTable("ParkingHasPrice");

            builder.HasOne(d => d.Parking)
                .WithMany(p => p.ParkingHasPrices)
                .HasForeignKey(d => d.ParkingId)
                .HasConstraintName("FK_ParkingHasPrice_Parking");

            builder.HasOne(d => d.ParkingPrice)
                .WithMany(p => p.ParkingHasPrices)
                .HasForeignKey(d => d.ParkingPriceId)
                .HasConstraintName("FK_ParkingHasPrice_ParkingPrice");
        }
    }
}

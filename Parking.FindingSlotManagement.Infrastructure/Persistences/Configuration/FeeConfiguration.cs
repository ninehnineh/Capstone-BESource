using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Configuration
{
    public class FeeConfiguration : IEntityTypeConfiguration<Fee>
    {
        public void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.HasData(
                new Fee { FeeId = 1, BusinessType = "Tư nhân", Name = "Cước phí mặc định tư nhân", NumberOfParking = "1", Price = 100000 },
                new Fee { FeeId = 2, BusinessType = "Doanh nghiệp", Name = "Cước phí mặc định doanh nghiệp", NumberOfParking = "Unlimited", Price = 500000}
            );

            builder.Property(x => x.NumberOfParking).HasMaxLength(20);
        }
    }
}
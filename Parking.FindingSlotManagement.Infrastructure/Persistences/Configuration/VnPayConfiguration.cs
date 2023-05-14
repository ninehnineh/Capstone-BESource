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
    public class VnPayConfiguration : IEntityTypeConfiguration<VnPay>
    {
        public void Configure(EntityTypeBuilder<VnPay> builder)
        {
            builder.ToTable("VnPay");

            builder.HasIndex(e => e.ManagerId, "IX_VnPay_ManagerID");

            builder.Property(e => e.HashSecret)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.ManagerId).HasColumnName("ManagerID");

            builder.Property(e => e.TmnCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.Manager)
                .WithMany(p => p.VnPays)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__VnPay__ManagerID__2A4B4B5E");
        }
    }
}

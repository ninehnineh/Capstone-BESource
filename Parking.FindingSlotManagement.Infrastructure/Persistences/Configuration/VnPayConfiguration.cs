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
    public class VnPayConfiguration : IEntityTypeConfiguration<Domain.Entities.VnPay>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.VnPay> builder)
        {
            builder.ToTable("VnPay");

            builder.HasIndex(e => e.BusinessId, "IX_VnPay_BusinessId");

            builder.Property(e => e.HashSecret)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.BusinessId).HasColumnName("BusinessId");

            builder.Property(e => e.TmnCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.Business)
                .WithMany(p => p.VnPays)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__VnPay__BusinessId__2A4B4B5E");
        }
    }
}

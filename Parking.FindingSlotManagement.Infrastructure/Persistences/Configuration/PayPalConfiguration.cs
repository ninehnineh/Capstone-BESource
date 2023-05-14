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
    public class PayPalConfiguration : IEntityTypeConfiguration<PayPal>
    {
        public void Configure(EntityTypeBuilder<PayPal> builder)
        {
            builder.ToTable("PayPal");

            builder.HasIndex(e => e.ManagerId, "IX_PayPal_ManagerID");

            builder.Property(e => e.ClientId)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.ManagerId).HasColumnName("ManagerID");

            builder.Property(e => e.SecretKey)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.HasOne(d => d.Manager)
                .WithMany(p => p.PayPals)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__PayPal__ManagerI__2D27B809");
        }
    }
}

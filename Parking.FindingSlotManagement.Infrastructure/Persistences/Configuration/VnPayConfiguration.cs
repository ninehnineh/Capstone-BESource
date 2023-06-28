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

            builder.HasIndex(e => e.UserId, "IX_userId_VnPay");

            builder.Property(e => e.HashSecret)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.UserId).HasColumnName("UserId");

            builder.Property(e => e.TmnCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.User)
                .WithMany(p => p.VnPays)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__VnPay__User");


        }
    }
}

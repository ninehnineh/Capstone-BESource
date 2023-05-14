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
    public class OTPConfiguration : IEntityTypeConfiguration<OTP>
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.ToTable("OTPs");

            builder.HasIndex(e => e.UserId, "IX_OTPs_UserId");

            builder.Property(e => e.OTPID).HasColumnName("OTPID");

            builder.Property(e => e.Code)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.ExpirationTime).HasColumnType("datetime");

            builder.HasOne(d => d.User)
                .WithMany(p => p.OTPs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OTP__UserID");
        }
    }
}

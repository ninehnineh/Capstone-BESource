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
    public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
    {
        public void Configure(EntityTypeBuilder<BusinessProfile> builder)
        {
            builder.HasIndex(e => e.UserId, "UQ__Business__1788CCAD877AB68C")
                    .IsUnique()
                    .HasFilter("([UserID] IS NOT NULL)");

            builder.ToTable("Business");

            builder.Property(e => e.Address).HasMaxLength(255);

            builder.Property(e => e.BackIdentification)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.BusinessLicense)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.FrontIdentification)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Name).HasMaxLength(255);
                
            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.User)
                .WithOne(p => p.BusinessProfile)
                .HasForeignKey<BusinessProfile>(d => d.UserId)
                .HasConstraintName("fk_IsManager");

            builder.HasOne(d => d.Fee)
                .WithMany(x => x.BusinessProfiles)
                .HasForeignKey(x => x.FeeId)
                .HasConstraintName("FK_Fee_BusinessProfiles");
        }
    }
}

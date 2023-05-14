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
    public class FavoriteAddressConfiguration : IEntityTypeConfiguration<FavoriteAddress>
    {
        public void Configure(EntityTypeBuilder<FavoriteAddress> builder)
        {
            builder.ToTable("FavoriteAddress");

            builder.HasIndex(e => e.UserId, "IX_FavoriteAddress_UserID");
                
            builder.Property(e => e.Address).HasMaxLength(255);

            builder.Property(e => e.TagName).HasMaxLength(20);

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.User)
                .WithMany(p => p.FavoriteAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FavoriteA__UserI__33D4B598");
        }
    }
}

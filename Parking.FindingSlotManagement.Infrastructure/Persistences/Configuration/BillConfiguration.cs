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
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasOne(x => x.businessProfile)
                .WithMany(x => x.Bills)
                .HasForeignKey(x => x.BusinessId)
                .HasConstraintName("FK_businessPro_Bills");

            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.Bills)
                .HasForeignKey(x => x.WalletId)
                .HasConstraintName("FK_Wallet_Bills");
        }
    }
}

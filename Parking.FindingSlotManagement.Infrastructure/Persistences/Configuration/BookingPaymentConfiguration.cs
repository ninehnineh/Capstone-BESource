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
    public class BookingPaymentConfiguration : IEntityTypeConfiguration<BookingPayment>
    {
        public void Configure(EntityTypeBuilder<BookingPayment> builder)
        {
            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.BookingPayments)
                .HasForeignKey(x => x.WalletId)
                .HasConstraintName("FK_Wallet_BookingPayments");

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingPayments)
                .HasPrincipalKey(x => x.BookingId)
                .HasConstraintName("FK_Booking_BookingPayments");
        }
    }
}

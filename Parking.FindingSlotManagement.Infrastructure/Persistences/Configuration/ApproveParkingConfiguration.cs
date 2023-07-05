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
    public class ApproveParkingConfiguration : IEntityTypeConfiguration<ApproveParking>
    {
        public void Configure(EntityTypeBuilder<ApproveParking> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.ApproveParkings)
                .HasForeignKey(x => x.StaffId)
                .HasConstraintName("FK_User_ApproveParkings");

            builder.HasOne(x => x.Parking)
                .WithMany(x => x.ApproveParkings)
                .HasForeignKey(x => x.ParkingId)
                .HasConstraintName("FK_Parking_ApproveParkings");


        }
    }
}

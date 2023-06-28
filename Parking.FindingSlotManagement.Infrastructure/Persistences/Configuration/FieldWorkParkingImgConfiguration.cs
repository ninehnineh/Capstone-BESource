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
    public class FieldWorkParkingImgConfiguration : IEntityTypeConfiguration<FieldWorkParkingImg>
    {
        public void Configure(EntityTypeBuilder<FieldWorkParkingImg> builder)
        {
            builder.HasOne(x => x.ApproveParking)
                .WithMany(x => x.FieldWorkParkingImgs)
                .HasForeignKey(X => X.ApproveParkingId)
                .HasConstraintName("FK_ApprovePar_FieldWorkPas");
        }
    }
}

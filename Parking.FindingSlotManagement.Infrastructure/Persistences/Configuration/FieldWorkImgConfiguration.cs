//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Parking.FindingSlotManagement.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Configuration
//{
//    public class FieldWorkImgConfiguration : IEntityTypeConfiguration<FieldWorkImg>
//    {
//        public void Configure(EntityTypeBuilder<FieldWorkImg> builder)
//        {
//            builder.ToTable("FieldWorkImg");

//            builder.Property(e => e.ImgUrl)
//                .HasMaxLength(255)
//                .IsUnicode(false);

//            builder.HasOne(d => d.BusinessProfile)
//                .WithMany(p => p.FieldWorkImgs)
//                .HasForeignKey(d => d.BusinessProfileId)
//                .HasConstraintName("FK_FieldWorkImg_Business");
//        }
//    }
//}

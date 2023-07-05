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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.ManagerId, "IX_Users_ManagerID");

            builder.HasIndex(e => e.RoleId, "IX_Users_RoleID");

            builder.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.DateOfBirth).HasColumnType("date");

            builder.Property(e => e.Devicetoken)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Gender)
                .HasMaxLength(20);

            builder.Property(e => e.ManagerId).HasColumnName("ManagerID");

            builder.Property(e => e.Name).HasMaxLength(50);

            builder.Property(e => e.PasswordHash).HasMaxLength(255);

            builder.Property(e => e.PasswordSalt).HasMaxLength(255);

            builder.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.IdCardNo)
                .HasMaxLength(20);

            builder.Property(e => e.IdCardDate)
                .HasColumnType("date");

            builder.Property(e => e.IdCardIssuedBy)
                .HasMaxLength(255);

            builder.Property(e => e.Address)
                .HasMaxLength(255);

            builder.Property(e => e.RoleId).HasColumnName("RoleID");

            builder.HasOne(d => d.Manager)
                .WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Users__ManagerID__267ABA7A");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__276EDEB3");

            builder.HasOne(x => x.Parking)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ParkingId)
                .HasConstraintName("FK__Parking__Users");

        }
    }
}

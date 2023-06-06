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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(20);

            builder.HasData(
                    new Role {RoleId = 1, Name = "Manager", IsActive = true}, 
                    new Role {RoleId = 2, Name = "Keeper", IsActive = true}, 
                    new Role {RoleId = 3, Name = "Customer", IsActive = true},
                    new Role {RoleId = 4, Name = "Staff", IsActive = true});
        }
    }
}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences
{
    public class ParkZDbContextFactory : IDesignTimeDbContextFactory<ParkZDbContext>
    {
        public ParkZDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\Parking.FindingSlotManagement.Api"))
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            var builder = new DbContextOptionsBuilder<ParkZDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new ParkZDbContext(builder.Options);
        }
    }
}

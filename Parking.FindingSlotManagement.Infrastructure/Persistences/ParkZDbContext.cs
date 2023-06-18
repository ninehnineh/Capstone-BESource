using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences
{
    public class ParkZDbContext : DbContext
    {
        public ParkZDbContext(DbContextOptions<ParkZDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<BusinessProfile> BusinessProfiles { get; set; } = null!;
        public DbSet<FieldWorkImg> FieldWorkImgs { get; set; } = null!;
        public DbSet<FavoriteAddress> FavoriteAddresses { get; set; } = null!;
        public DbSet<Floor> Floors { get; set; } = null!;
        public DbSet<TimeLine> TimeLines { get; set; } = null!;
        public DbSet<Domain.Entities.Parking> Parkings { get; set; } = null!;
        public DbSet<ParkingHasPrice> ParkingHasPrices { get; set; } = null!;
        public DbSet<ParkingPrice> ParkingPrices { get; set; } = null!;
        public DbSet<ParkingSlot> ParkingSlots { get; set; } = null!;
        public DbSet<ParkingSpotImage> ParkingSpotImages { get; set; } = null!;
        public DbSet<PayPal> PayPals { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<StaffParking> StaffParkings { get; set; } = null!;
        public DbSet<Traffic> Traffics { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<VehicleInfor> VehicleInfors { get; set; } = null!;
        public DbSet<Domain.Entities.VnPay> VnPays { get; set; } = null!;
        public DbSet<OTP> OTPs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            
            //OnModelCreatingPartial(modelBuilder);
        }
    }
}

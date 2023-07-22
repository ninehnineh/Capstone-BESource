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
        public DbSet<Traffic> Traffics { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<VehicleInfor> VehicleInfors { get; set; } = null!;
        public DbSet<Fee> Fees { get; set; } = null!;
        public DbSet<Bill> Bills { get; set; } = null!;
        public DbSet<Wallet> Wallets { get; set; } = null!;
        public DbSet<FieldWorkParkingImg> FieldWorkParkingImgs { get; set; } = null!;
        public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<ApproveParking> ApproveParkings { get; set; } = null!;
        public DbSet<Domain.Entities.VnPay> VnPays { get; set; } = null!;
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<ConflictRequest> ConflictRequests { get; set; }


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

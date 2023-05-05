using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<PackagePrice> PackagePrices { get; set; } = null!;
        public DbSet<Domain.Entities.Parking> Parkings { get; set; } = null!;
        public DbSet<ParkingHasPrice> ParkingHasPrices { get; set; } = null!;
        public DbSet<ParkingSlot> ParkingSlots { get; set; } = null!;
        public DbSet<ParkingSpotImage> ParkingSpotImages { get; set; } = null!;
        public DbSet<PayPal> PayPals { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<StaffParking> StaffParkings { get; set; } = null!;
        public DbSet<Traffic> Traffics { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<VehicleInfor> VehicleInfors { get; set; } = null!;
        public DbSet<VnPay> VnPays { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var builder = new ConfigurationBuilder()
            //                .SetBasePath(Directory.GetCurrentDirectory())
            //                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //IConfigurationRoot configuration = builder.Build();
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.ParkingSlotId, e.StartTime, e.DateBook })
                    .HasName("PK__Booking__1BDD09E6ABAB9F2E");

                entity.ToTable("Booking");

                entity.HasIndex(e => e.BookingId, "UQ__Booking__3214EC2628BBAE14")
                    .IsUnique();

                entity.Property(e => e.ParkingSlotId).HasColumnName("ParkingSlotID");

                entity.Property(e => e.DateBook).HasColumnType("date");

                entity.Property(e => e.ActualPrice).HasColumnType("money");

                entity.Property(e => e.GuestName).HasMaxLength(50);

                entity.Property(e => e.GuestPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BookingId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BookingID");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(225)
                    .IsUnicode(false);

                entity.Property(e => e.QrcodeText)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("QRCodeText");

                entity.Property(e => e.TmnCodeVnPay)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalPrice).HasColumnType("money");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VehicleInforId).HasColumnName("VehicleInforID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Booking__UserID__5070F446");

                entity.HasOne(d => d.VehicleInfor)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.VehicleInforId)
                    .HasConstraintName("FK__Booking__Vehicle__4F7CD00D");
            });

            modelBuilder.Entity<BusinessProfile>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ__Business__1788CCAD877AB68C")
                    .IsUnique();

                entity.Property(e => e.BusinessProfileId).HasColumnName("BusinessProfileId");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.BackIdentification)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessLicense)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FrontIdentification)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.BusinessProfile)
                    .HasForeignKey<BusinessProfile>(d => d.UserId)
                    .HasConstraintName("fk_IsManager");
            });

            modelBuilder.Entity<FavoriteAddress>(entity =>
            {
                entity.ToTable("FavoriteAddress");

                entity.Property(e => e.FavoriteAddressId).HasColumnName("FavoriteAddressId");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.TagName).HasMaxLength(20);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteAddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FavoriteA__UserI__33D4B598");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.Property(e => e.FloorId).HasColumnName("FloorId");

                entity.Property(e => e.FloorName).HasMaxLength(50);

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.Floors)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__Floors__ParkingI__47DBAE45");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("NotificationId");

                entity.Property(e => e.Body).HasMaxLength(225);

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Tiltle).HasMaxLength(225);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Notifications)
                    .HasPrincipalKey(p => p.BookingId)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Notificat__Booki__59063A47");
            });

            modelBuilder.Entity<PackagePrice>(entity =>
            {
                entity.ToTable("PackagePrice");

                entity.Property(e => e.PackagePriceId).HasColumnName("PackagePriceId");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EndTime).HasColumnType("time");

                entity.Property(e => e.ExtraFee).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PenaltyPrice).HasColumnType("money");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StartTime).HasColumnType("time");

                entity.Property(e => e.TrafficId).HasColumnName("TrafficID");

                entity.HasOne(d => d.Traffic)
                    .WithMany(p => p.PackagePrices)
                    .HasForeignKey(d => d.TrafficId)
                    .HasConstraintName("FK__PackagePr__Traff__412EB0B6");
            });

            modelBuilder.Entity<Domain.Entities.Parking>(entity =>
            {
                entity.ToTable("Parking");

                entity.Property(e => e.ParkingId).HasColumnName("ParkingId");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Code)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Latitude).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ParkingHasPrice>(entity =>
            {
                entity.ToTable("ParkingHasPrice");

                entity.Property(e => e.ParkingHasPriceId).HasColumnName("ParkingHasPriceId");

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.Property(e => e.ParkingPriceId).HasColumnName("ParkingPriceID");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.ParkingHasPrices)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__ParkingHa__Parki__440B1D61");

                entity.HasOne(d => d.ParkingPrice)
                    .WithMany(p => p.ParkingHasPrices)
                    .HasForeignKey(d => d.ParkingPriceId)
                    .HasConstraintName("FK__ParkingHa__Parki__44FF419A");
            });

            modelBuilder.Entity<ParkingSlot>(entity =>
            {
                entity.Property(e => e.ParkingSlotId).HasColumnName("ParkingSlotId");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.FloorId).HasColumnName("FloorID");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.TrafficId).HasColumnName("TrafficID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.ParkingSlots)
                    .HasPrincipalKey(p => p.BookingId)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__ParkingSl__Booki__5629CD9C");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.ParkingSlots)
                    .HasForeignKey(d => d.FloorId)
                    .HasConstraintName("FK__ParkingSl__Floor__5441852A");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.ParkingSlots)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__ParkingSl__Parki__5535A963");

                entity.HasOne(d => d.Traffic)
                    .WithMany(p => p.ParkingSlots)
                    .HasForeignKey(d => d.TrafficId)
                    .HasConstraintName("FK__ParkingSl__Traff__534D60F1");
            });

            modelBuilder.Entity<ParkingSpotImage>(entity =>
            {
                entity.ToTable("ParkingSpotImage");

                entity.Property(e => e.ParkingSpotImageId).HasColumnName("ParkingSpotImageId");

                entity.Property(e => e.ImgPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.ParkingSpotImages)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__ParkingSp__Parki__3C69FB99");
            });

            modelBuilder.Entity<PayPal>(entity =>
            {
                entity.ToTable("PayPal");

                entity.Property(e => e.PayPalId).HasColumnName("PayPalId");

                entity.Property(e => e.ClientId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.SecretKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.PayPals)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__PayPal__ManagerI__2D27B809");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<StaffParking>(entity =>
            {
                entity.ToTable("StaffParking");

                entity.Property(e => e.StaffParkingId).HasColumnName("StaffParkingId");

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.StaffParkings)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__StaffPark__Parki__398D8EEE");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StaffParkings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__StaffPark__UserI__38996AB5");
            });

            modelBuilder.Entity<Traffic>(entity =>
            {
                entity.ToTable("Traffic");

                entity.Property(e => e.TrafficId).HasColumnName("TrafficId");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Devicetoken)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                    
                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__Users__ManagerID__267ABA7A");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleID__276EDEB3");
            });

            modelBuilder.Entity<VehicleInfor>(entity =>
            {
                entity.ToTable("VehicleInfor");

                entity.Property(e => e.VehicleInforId).HasColumnName("VehicleInforId");

                entity.Property(e => e.Color).HasMaxLength(225);

                entity.Property(e => e.LicensePlate).HasMaxLength(225);

                entity.Property(e => e.TrafficId).HasColumnName("TrafficID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VehicleName).HasMaxLength(50);

                entity.HasOne(d => d.Traffic)
                    .WithMany(p => p.VehicleInfors)
                    .HasForeignKey(d => d.TrafficId)
                    .HasConstraintName("FK__VehicleIn__Traff__4BAC3F29");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VehicleInfors)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__VehicleIn__UserI__4AB81AF0");
            });

            modelBuilder.Entity<VnPay>(entity =>
            {
                entity.ToTable("VnPay");

                entity.Property(e => e.VnPayId).HasColumnName("VnPayId");

                entity.Property(e => e.HashSecret)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.TmnCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.VnPays)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__VnPay__ManagerID__2A4B4B5E");
            });

            //OnModelCreatingPartial(modelBuilder);
        }
    }
}

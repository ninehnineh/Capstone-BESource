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
        public DbSet<Notification> Notifications { get; set; } = null!;
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
        public DbSet<VnPay> VnPays { get; set; } = null!;
        public DbSet<OTP> OTPs { get; set; } = null!;

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

                entity.HasIndex(e => e.BookingId, "AK_Booking_BookingIDsas")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "IX_Booking_UserID");

                entity.HasIndex(e => e.VehicleInforId, "IX_Booking_VehicleInforID");

                entity.HasIndex(e => e.BookingId, "UQ__Booking__3214EC2628BBAE14")
                    .IsUnique();

                entity.Property(e => e.ParkingSlotId).HasColumnName("ParkingSlotID");

                entity.Property(e => e.DateBook).HasColumnType("date");

                entity.Property(e => e.ActualPrice).HasColumnType("money");

                entity.Property(e => e.BookingId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BookingID");

                entity.Property(e => e.GuestName).HasMaxLength(50);

                entity.Property(e => e.GuestPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

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
                    .IsUnique()
                    .HasFilter("([UserID] IS NOT NULL)");

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

                entity.HasIndex(e => e.UserId, "IX_FavoriteAddress_UserID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.TagName).HasMaxLength(20);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteAddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FavoriteA__UserI__33D4B598");
            });

            modelBuilder.Entity<FieldWorkImg>(entity =>
            {
                entity.ToTable("FieldWorkImg");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessProfile)
                    .WithMany(p => p.FieldWorkImgs)
                    .HasForeignKey(d => d.BusinessProfileId)
                    .HasConstraintName("FK_FieldWorkImg_BusinessProfiles");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.HasIndex(e => e.ParkingId, "IX_Floors_ParkingID");

                entity.Property(e => e.FloorName).HasMaxLength(50);

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.Floors)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK__Floors__ParkingI__47DBAE45");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(e => e.BookingId, "IX_Notifications_BookingID");

                entity.Property(e => e.Body).HasMaxLength(225);

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Tiltle).HasMaxLength(225);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Notifications)
                    .HasPrincipalKey(p => p.BookingId)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Notificat__Booki__59063A47");
            });

            modelBuilder.Entity<OTP>(entity =>
            {
                entity.ToTable("OTPs");

                entity.HasIndex(e => e.UserId, "IX_OTPs_UserId");

                entity.Property(e => e.OTPID).HasColumnName("OTPID");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExpirationTime).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OTPs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__OTP__UserID");
            });

            modelBuilder.Entity<Domain.Entities.Parking>(entity =>
            {
                entity.ToTable("Parking");

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

                entity.HasOne(d => d.Parking)
                    .WithMany(p => p.ParkingHasPrices)
                    .HasForeignKey(d => d.ParkingId)
                    .HasConstraintName("FK_ParkingHasPrice_Parking");

                entity.HasOne(d => d.ParkingPrice)
                    .WithMany(p => p.ParkingHasPrices)
                    .HasForeignKey(d => d.ParkingPriceId)
                    .HasConstraintName("FK_ParkingHasPrice_ParkingPrice");
            });

            modelBuilder.Entity<ParkingPrice>(entity =>
            {
                entity.ToTable("ParkingPrice");

                entity.Property(e => e.ParkingPriceName).HasMaxLength(250);

            });

            modelBuilder.Entity<ParkingPrice>()
                    .Property(x => x.UserId)
                    .HasColumnName("BusinessId");


            modelBuilder.Entity<ParkingPrice>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.ParkingPrices)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Business__ParkingPri__sdwq23dca");

            modelBuilder.Entity<ParkingSlot>(entity =>
            {
                entity.HasIndex(e => e.BookingId, "IX_ParkingSlots_BookingID");

                entity.HasIndex(e => e.FloorId, "IX_ParkingSlots_FloorID");

                entity.HasIndex(e => e.ParkingId, "IX_ParkingSlots_ParkingID");

                entity.HasIndex(e => e.TrafficId, "IX_ParkingSlots_TrafficID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.FloorId).HasColumnName("FloorID");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ParkingId).HasColumnName("ParkingID");

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

                entity.HasIndex(e => e.ParkingId, "IX_ParkingSpotImage_ParkingID");

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

                entity.HasIndex(e => e.ManagerId, "IX_PayPal_ManagerID");

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
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<StaffParking>(entity =>
            {
                entity.ToTable("StaffParking");

                entity.HasIndex(e => e.ParkingId, "IX_StaffParking_ParkingID");

                entity.HasIndex(e => e.UserId, "IX_StaffParking_UserID");

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

            modelBuilder.Entity<TimeLine>(entity =>
            {
                entity.ToTable("TimeLine");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ExtraFee).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PenaltyPrice).HasColumnType("money");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StartingTime).HasColumnType("datetime");

                entity.HasOne(d => d.ParkingPrice)
                    .WithMany(p => p.TimeLines)
                    .HasForeignKey(d => d.ParkingPriceId)
                    .HasConstraintName("FK_Timeline_ParkingPrice");

                entity.HasOne(d => d.Traffic)
                    .WithMany(p => p.TimeLines)
                    .HasForeignKey(d => d.TrafficId)
                    .HasConstraintName("FK_Timeline_Traffic");
            });

            modelBuilder.Entity<Traffic>(entity =>
            {
                entity.ToTable("Traffic");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.ManagerId, "IX_Users_ManagerID");

                entity.HasIndex(e => e.RoleId, "IX_Users_RoleID");

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

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.PasswordSalt).HasMaxLength(255);

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

                entity.HasIndex(e => e.TrafficId, "IX_VehicleInfor_TrafficID");

                entity.HasIndex(e => e.UserId, "IX_VehicleInfor_UserID");

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

                entity.HasIndex(e => e.ManagerId, "IX_VnPay_ManagerID");

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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Infrastructure.Mail;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using Parking.FindingSlotManagement.Infrastructure.Repositories;
using Parking.FindingSlotManagement.Infrastructure.Repositories.AuthenticationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ParkZDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBusinessManagerAuthenticationRepository, BusinessManagerAuthenticationRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITrafficRepository, TrafficRepository>();
            services.AddScoped<IParkingRepository, ParkingRepository>();
            services.AddScoped<IStaffParkingRepository, StaffParkingRepository>();
            services.AddScoped<IFloorRepository, FloorRepository>();
            services.AddScoped<IFavoriteAddressRepository, FavoriteAddressRepository>();
            services.AddScoped<IVehicleInfoRepository, VehicleInfoRepository>();
            services.AddScoped<IBusinessProfileRepository, BusinessProfileRepository>();
            services.AddScoped<IAdminAuthenticationRepository, AdminAuthenticationRepository>();
            services.AddScoped<IStaffAuthenticationRepository, StaffAuthenticationRepository>();
            services.AddScoped<IPackagePriceRepository, PackagePriceRepository>();
            services.AddScoped<IVnPayRepository, VnPayRepository>();
            services.AddScoped<IParkingHasPriceRepository, ParkingHasPriceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IParkingSlotRepository, ParkingSlotRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IOTPRepository, OTPRepository>();
            services.AddScoped<IPaypalRepository, PaypalRepository>();
            services.AddScoped<IParkingSpotImageRepository, ParkingSpotImageRepository>();
            return services;
        }
    }
}

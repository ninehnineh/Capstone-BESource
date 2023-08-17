using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Exceptions;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate.Model;
using Parking.FindingSlotManagement.Application.Models.Parking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class ParkingRepository : GenericRepository<Domain.Entities.Parking>, IParkingRepository
    {
        private readonly ParkZDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private string connectionString;

        public ParkingRepository(ParkZDbContext dbContext, IMapper mapper, IConfiguration configuration) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task EnableDisableParkingById(int parkingId, DateTime disableDate)
        {
            try
            {
                var disableParkingIncludeTimeSlots = await dbContext.Parkings
                    .Include(x => x.Floors)!.ThenInclude(x => x.ParkingSlots)!.ThenInclude(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

                var floors = disableParkingIncludeTimeSlots!.Floors!;
                disableParkingIncludeTimeSlots.IsAvailable = true;
                foreach (var floor in floors)
                {
                    var parkingSlots = floor!.ParkingSlots!;
                    foreach (var parkingSlot in parkingSlots)
                    {
                        var timeSlots = parkingSlot.TimeSlots;
                        foreach (var timeSlot in timeSlots)
                        {
                            if (timeSlot.StartTime.Date == disableDate.Date)
                            {
                                timeSlot.Status = TimeSlotStatus.Free.ToString();
                            }
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetDisableParkingById: Message {ex.Message}");
            }
        }

        public async Task<DisableSlotParking> GetParking(int parkingSlotId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(parkingSlotId);

                var parking = await dbContext.Parkings
                    .Include(x => x.BusinessProfile!.User)
                    .FirstOrDefaultAsync(p => p.Floors!
                        .Any(f => f.ParkingSlots!
                            .Any(ps => ps.ParkingSlotId == parkingSlotId)
                        )
                    );

                var response = new DisableSlotParking { ParkingId = parking!.ParkingId, ManagerId = parking.BusinessProfile.UserId!.Value };

                return response;
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at GetParking: Message {ex.Message}");
            }
        }

        public async Task<Domain.Entities.Parking> GetParkingById(int parkingId)
        {
            try
            {

                var parking = await dbContext.Parkings
                    .Include(x => x.Floors)!.ThenInclude(x => x.ParkingSlots)!.ThenInclude(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

                return parking;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetParkingById: Message {ex.Message}");
            }
        }

        public async Task<bool> GetDisableParking(int parkingId, DateTime disableDate)
        {
            string methodName = "DisableParkingByDate";
            try
            {
                var result = false;
                // var query = "SELECT * " +
                //             "FROM HangFire.Job " +
                //             "WHERE JSON_VALUE(HangFire.Job.InvocationData, '$.m') = @MethodName AND JSON_VALUE(HangFire.Job.Arguments, '$[0]') = @ParkingId AND HangFire.Job.Arguments LIKE '%' + @DisableDate + '%'";
                var query = "SELECT * " +
                            "FROM HangFire.Job " +
                            "WHERE JSON_VALUE(HangFire.Job.InvocationData, '$.m') = @MethodName AND JSON_VALUE(HangFire.Job.Arguments, '$[0]') = @ParkingId AND JSON_VALUE(HangFire.Job.Arguments, '$[1]') LIKE '%' + @DisableDate + '%'";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MethodName", methodName);
                        command.Parameters.AddWithValue("@ParkingId", parkingId.ToString());
                        command.Parameters.AddWithValue("@DisableDate", disableDate.ToString("yyyy-MM-dd"));

                        using (var reader = command.ExecuteReader())
                        {
                            result = reader.HasRows;
                            reader.Close();
                        };
                    };
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetHistoryDisableParking: Message {ex.Message}");
            }
        }

        public async Task<int> GetManagerIdByParkingId(int parkingId)
        {
            try
            {
                var parking = await dbContext.Parkings
                    .Include(x => x.BusinessProfile)
                    .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

                var managerId = parking!.BusinessProfile.UserId;

                return managerId.Value;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetManagerIdByParkingId: Message {ex.Message}");
            }
        }

        public async Task DisableParkingById(int parkingId)
        {
            try
            {
                var parking = await dbContext.Parkings
                    .FindAsync(parkingId);

                if (parking == null)
                    throw new NotFoundException("parking", "parkingId");

                parking.IsAvailable = false;
                
                await dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingById: Message {ex.Message}");
            }
        }

    }
}

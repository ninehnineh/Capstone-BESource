using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId
{
    public class GetRevenueByParkingIdQueryHandler : IRequestHandler<GetRevenueByParkingIdQuery, ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetRevenueByParkingIdQueryHandler(IBookingRepository bookingRepository, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>> Handle(GetRevenueByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                List<GetRevenueByParkingIdResponse> lstRes = new();
                DateTime currentDate = DateTime.Today; // Get the current date
                if (!string.IsNullOrEmpty(request.Week))
                {


                    // Find the start and end dates of the current week
                    int diff = (7 + (currentDate.DayOfWeek - DayOfWeek.Monday)) % 7;
                    DateTime startDate = currentDate.AddDays(-diff);
                    DateTime endDate = startDate.AddDays(6);

                    // Get all the dates in the week
                    List<DateTime> weekDates = new List<DateTime>();
                    DateTime currentDatee = startDate;

                    while (currentDatee <= endDate)
                    {
                        weekDates.Add(currentDatee);
                        currentDatee = currentDatee.AddDays(1);
                    }

                    // Print all the dates
                    foreach (DateTime weekDate in weekDates)
                    {
                        GetRevenueByParkingIdResponse entity = new()
                        {
                            Date = weekDate.Date
                        };
                        var totalMoneyOfTheDay = 0M;

                        var money = await _bookingRepository.GetRevenueByDateByParkingIdMethod(parkingExist.ParkingId, weekDate.Date);
                        totalMoneyOfTheDay += money;
                        entity.RevenueOfTheDate = totalMoneyOfTheDay;
                        lstRes.Add(entity);
                    }

                }
                if (!string.IsNullOrEmpty(request.Month))
                {

                    int year = currentDate.Year; // Year of the current date
                    int month = currentDate.Month; // Month of the current date

                    int daysInMonth = DateTime.DaysInMonth(year, month); // Number of days in the current month

                    List<DateTime> allDates = new List<DateTime>();

                    // Loop through each date in the current month and add it to the list
                    for (int day = 1; day <= daysInMonth; day++)
                    {
                        DateTime date = new DateTime(year, month, day);
                        allDates.Add(date);
                    }

                    // Print all the dates
                    foreach (DateTime date in allDates)
                    {
                        GetRevenueByParkingIdResponse entity = new()
                        {
                            Date = date.Date
                        };
                        var totalMoneyOfTheDay = 0M;
                        var money = await _bookingRepository.GetRevenueByDateByParkingIdMethod(parkingExist.ParkingId, date.Date);
                        totalMoneyOfTheDay += money;
                        entity.RevenueOfTheDate = totalMoneyOfTheDay;
                        lstRes.Add(entity);
                    }
                }
                return new ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Data = lstRes
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

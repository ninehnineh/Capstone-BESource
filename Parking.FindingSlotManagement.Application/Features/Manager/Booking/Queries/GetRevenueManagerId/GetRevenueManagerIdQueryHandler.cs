using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueManagerId
{
    public class GetRevenueManagerIdQueryHandler : IRequestHandler<GetRevenueManagerIdQuery, ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetRevenueManagerIdQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>> Handle(GetRevenueManagerIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var managerExist = await _userRepository.GetById(request.ManagerId);
                if (managerExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (managerExist.RoleId != 1)
                {
                    return new ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>
                    {
                        Message = "Tài khoản không phải là quản lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);
                if (businessExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                //get list parking
                var lstParking = await _parkingRepository.GetAllItemWithCondition(x => x.BusinessId == businessExist.BusinessProfileId);
                if (!lstParking.Any())
                {
                    return new ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                List<GetRevenueManagerIdResponse> lstRes = new();
                DateTime currentDate = DateTime.Today; // Get the current date
                if(!string.IsNullOrEmpty(request.Week))
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
                    foreach (DateTime weekDate in   weekDates)
                    {
                        GetRevenueManagerIdResponse entity = new()
                        {
                            Date = weekDate.Date
                        };
                        var totalMoneyOfTheDay = 0M;
                        foreach (var item in lstParking)
                        {
                            var money = await _bookingRepository.GetRevenueByDateByParkingIdMethod(item.ParkingId, weekDate.Date);
                            totalMoneyOfTheDay += money;
                        }
                        entity.RevenueOfTheDate = totalMoneyOfTheDay;
                        lstRes.Add(entity);
                    }
                    
                }
                if(!string.IsNullOrEmpty(request.Month))
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
                        GetRevenueManagerIdResponse entity = new()
                        {
                            Date = date.Date
                        };
                        var totalMoneyOfTheDay = 0M;
                        foreach (var item in lstParking)
                        {
                            var money = await _bookingRepository.GetRevenueByDateByParkingIdMethod(item.ParkingId, date.Date);
                            totalMoneyOfTheDay += money;
                        }
                        entity.RevenueOfTheDate = totalMoneyOfTheDay;
                        lstRes.Add(entity);
                    }
                }
                return new ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>
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

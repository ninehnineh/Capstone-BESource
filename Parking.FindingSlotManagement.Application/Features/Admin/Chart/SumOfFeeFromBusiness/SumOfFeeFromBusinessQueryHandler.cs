using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfFeeFromBusiness
{
    public class SumOfFeeFromBusinessQueryHandler : IRequestHandler<SumOfFeeFromBusinessQuery, ServiceResponse<IEnumerable<SumOfFeeFromBusinessResponse>>>
    {
        private readonly IBillRepository _billRepository;

        public SumOfFeeFromBusinessQueryHandler(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        public async Task<ServiceResponse<IEnumerable<SumOfFeeFromBusinessResponse>>> Handle(SumOfFeeFromBusinessQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<SumOfFeeFromBusinessResponse> lstRes = new();
                DateTime currentDate = DateTime.Today; // Get the current date


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
                    SumOfFeeFromBusinessResponse entity = new()
                    {
                        Date = date.Date
                    };
                    var totalMoneyOfTheDay = 0M;
                    var money = await _billRepository.GetRevenueOfBusinessByDateMethod(date.Date);
                    totalMoneyOfTheDay += money;
                    entity.RevenueOfTheDate = totalMoneyOfTheDay;
                    lstRes.Add(entity);
                }
                return new ServiceResponse<IEnumerable<SumOfFeeFromBusinessResponse>>
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

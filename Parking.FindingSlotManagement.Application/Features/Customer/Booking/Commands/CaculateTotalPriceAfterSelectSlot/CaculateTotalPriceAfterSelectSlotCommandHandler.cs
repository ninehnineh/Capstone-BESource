using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CaculateTotalPriceAfterSelectSlot
{
    public class CaculateTotalPriceAfterSelectSlotCommandHandler : IRequestHandler<CaculateTotalPriceAfterSelectSlotCommand, ServiceResponse<decimal>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;

        public CaculateTotalPriceAfterSelectSlotCommandHandler(IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
        }

        public async Task<ServiceResponse<decimal>> Handle(CaculateTotalPriceAfterSelectSlotCommand request, CancellationToken cancellationToken)
        {

            var startTimeBooking = request.StartimeBooking;
            var endTimeBooking = request.StartimeBooking
                .AddHours(request.DesiredHour);
            var currentParkingId = request.ParkingId;
            

            try
            {
                

                List<Expression<Func<ParkingHasPrice, object>>> includes = new List<Expression<Func<ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice.TimeLines,
                };

                var parkingHasPrice = await _parkingHasPriceRepository
                    .GetAllItemWithCondition(x => x.Parking.ParkingId == currentParkingId &&
                    x.ParkingPrice.Traffic.TrafficId == 1, includes); 

                var parkingPrice = parkingHasPrice.FirstOrDefault().ParkingPrice;

                var timeLines = parkingPrice.TimeLines;

                var expectedPrice = CaculatePriceBooking
                    .CaculateExpectedPrice(startTimeBooking, endTimeBooking, parkingPrice, timeLines);

                return new ServiceResponse<decimal>
                {
                    Data = expectedPrice,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

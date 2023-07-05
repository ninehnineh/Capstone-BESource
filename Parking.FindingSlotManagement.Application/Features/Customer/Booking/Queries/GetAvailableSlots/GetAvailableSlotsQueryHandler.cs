/*using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots
//{
//    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, ServiceResponse<IEnumerable<GetAvailableSlotsResponse>>>
//    {
//        private readonly IBookingRepository _bookingRepository;
//        private readonly IParkingSlotRepository _parkingSlotRepository;
//        private readonly IMapper _mapper;
//        private readonly IFloorRepository _floorRepository;

//        public GetAvailableSlotsQueryHandler(IBookingRepository bookingRepository,
//            IParkingSlotRepository parkingSlotRepository,
//            IMapper mapper,
//            IFloorRepository floorRepository)
//        {
//            _bookingRepository = bookingRepository;
//            _parkingSlotRepository = parkingSlotRepository;
//            _mapper = mapper;
//            _floorRepository = floorRepository;
//        }

//        public async Task<ServiceResponse<IEnumerable<GetAvailableSlotsResponse>>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
//        {
//            const string bookingStatusCancel = "Cancel";
//            var bookedSlots = new List<int>();
//            var startTimeBooking = request.StartTimeBooking;
//            var endTimeBooking = request.StartTimeBooking.AddHours(request.DesireHour);
//            var parkingId = request.ParkingId;

//            try
//            {
//                var includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
//                {
//                    x => x.ParkingSlot,
//                    x => x.ParkingSlot.Floor,
//                    x => x.ParkingSlot.Floor.Parking,
//                };

//                var booking = await _bookingRepository
//                    .GetAllItemWithCondition(x => x.ParkingSlot.Floor.Parking.ParkingId == parkingId &&
//                    x.Status != bookingStatusCancel &&
//                    x.DateBook.Date <= startTimeBooking.Date, includes);

//                HashSet<int> listParkingSlotIdExist = new();

//                foreach (var item in booking)
//                {
//                    var bookedStartTime = item.StartTime.Hour;
//                    var bookedEndTime = item.EndTime.Value.Hour;
//                    if (startTimeBooking < item.EndTime && endTimeBooking > item.StartTime)
//                    {
//                        listParkingSlotIdExist.Add(item.ParkingSlotId);
//                    }
//                }

//                var includes3 = new List<Expression<Func<Domain.Entities.ParkingSlot, object>>>
//                {
//                    x => x.Floor,
//                };

//                var lstParkingSlot = await _parkingSlotRepository
//                    .GetAllItemWithCondition(x => x.Floor.ParkingId == parkingId, includes3);

//                var filterParkingSlot = lstParkingSlot
//                    .Where(item => !listParkingSlotIdExist.Contains(item.ParkingSlotId)).ToList();

//                var responses = _mapper.Map<IEnumerable<GetAvailableSlotsResponse>>(filterParkingSlot);

                return new ServiceResponse<IEnumerable<GetAvailableSlotsResponse>>
                {
                    Data = responses,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                    Count = responses.Count(),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
*/


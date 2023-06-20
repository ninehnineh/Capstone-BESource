using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetParkingSlots
{
    public class GetParkingSlotsQueryHandler : IRequestHandler<GetParkingSlotsQuery, ServiceResponse<IEnumerable<GetParkingSlotsResponse>>>
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;

        public GetParkingSlotsQueryHandler(IParkingSlotRepository parkingSlotRepository,
            IParkingRepository parkingRepository,
            IMapper mapper, 
            IBookingRepository bookingRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetParkingSlotsResponse>>> Handle(GetParkingSlotsQuery request, CancellationToken cancellationToken)
        {
            const string bookingStatusCancel = "Cancel";
            var bookedSlots = new List<int>();
            var startTimeBooking = request.StartTimeBooking;
            var endTimeBooking = request.StartTimeBooking.AddHours(request.DesireHour);
            var parkingId = request.ParkingId;

            try
            {
                var includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.ParkingSlot,
                    x => x.ParkingSlot.Floor,
                    x => x.ParkingSlot.Floor.Parking,
                };

                var booking = await _bookingRepository
                    .GetAllItemWithCondition(x => x.ParkingSlot.Floor.Parking.ParkingId == parkingId &&
                    x.Status != bookingStatusCancel &&
                    x.DateBook.Date <= startTimeBooking.Date, includes);

                HashSet<int> listParkingSlotIdExist = new();

                foreach (var item in booking)
                {
                    var bookedStartTime = item.StartTime.Hour;
                    var bookedEndTime = item.EndTime.Value.Hour;
                    if (startTimeBooking < item.EndTime && endTimeBooking > item.StartTime)
                    {
                        listParkingSlotIdExist.Add(item.ParkingSlotId);
                    }
                }

                var includes3 = new List<Expression<Func<Domain.Entities.ParkingSlot, object>>>
                {
                    x => x.Floor,
                    x => x.Traffic,
                };

                var lstParkingSlot = await _parkingSlotRepository
                    .GetAllItemWithCondition(x => x.Floor.ParkingId == parkingId, includes3);

                var filterParkingSlot = lstParkingSlot
                    .Where(item => !listParkingSlotIdExist.Contains(item.ParkingSlotId)).ToList();

                var responses = _mapper.Map<IEnumerable<GetParkingSlotsResponse>>(filterParkingSlot);

                return new ServiceResponse<IEnumerable<GetParkingSlotsResponse>>
                {
                    Data = responses,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetParkingSlots
{
    public class GetParkingSlotsQueryHandler : IRequestHandler<GetParkingSlotsQuery, ServiceResponse<GetParkingSlotsResponse>>
    {
        private readonly int Car = 1; 
        private readonly int MoTo = 2;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly GetParkingSlotsResponse _getParkingSlotsResponse;

        public GetParkingSlotsQueryHandler(IParkingSlotRepository parkingSlotRepository,
            IParkingRepository parkingRepository,
            IMapper mapper, 
            IBookingRepository bookingRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _getParkingSlotsResponse = new GetParkingSlotsResponse();
        }
        public async Task<ServiceResponse<GetParkingSlotsResponse>> Handle(GetParkingSlotsQuery request, CancellationToken cancellationToken)
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

                var parking = await _parkingRepository.GetById(parkingId);

                var totalNumberOfCarSpot = parking.CarSpot;
                var totalNumberOfMotoSpot = parking.MotoSpot;

                var availableCarSpot = filterParkingSlot.Where(x => x.TrafficId == Car).Count();
                var availableMoToSpot = filterParkingSlot.Where(x => x.TrafficId == MoTo).Count();
                //var responses = _mapper.Map<GetParkingSlotsResponse>(filterParkingSlot);
                var responses = new GetParkingSlotsResponse
                {
                    ParkingSlots = _mapper.Map<IEnumerable<ParkingSlotsDto>>(filterParkingSlot),
                    Car = new CarSpot
                    {
                        AvailableCarSlots = availableCarSpot,
                        TotalNumberCarSlots = (int) totalNumberOfCarSpot
                    },
                    MoTo = new MoToSpot
                    {
                        AvailableMoToSlots = availableMoToSpot,
                        TotalNumberMoToSlots = (int) totalNumberOfMotoSpot
                    },
                };

                return new ServiceResponse<GetParkingSlotsResponse>
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

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetNumberOfBookedSlots
{
    public class GetNumberOfBookedSlotsQueryHandler : IRequestHandler<GetNumberOfBookedSlotsQuery, ServiceResponse<int>>
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetNumberOfBookedSlotsQueryHandler(IParkingSlotRepository parkingSlotRepository,
            IBookingRepository bookingRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
            _bookingRepository = bookingRepository;
        }
        public Task<ServiceResponse<int>> Handle(GetNumberOfBookedSlotsQuery request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}

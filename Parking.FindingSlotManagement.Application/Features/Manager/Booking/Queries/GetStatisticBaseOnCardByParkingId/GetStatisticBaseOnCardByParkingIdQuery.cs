using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId
{
    public class GetStatisticBaseOnCardByParkingIdQuery : IRequest<ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>>
    {
        public int ParkingId { get; set; }
    }
}

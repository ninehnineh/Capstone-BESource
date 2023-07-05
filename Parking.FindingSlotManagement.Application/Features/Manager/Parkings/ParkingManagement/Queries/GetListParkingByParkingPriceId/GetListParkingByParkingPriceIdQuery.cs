using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByParkingPriceId
{
    public class GetListParkingByParkingPriceIdQuery : IRequest<ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>>
    {
        public int ParkingPriceId { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId
{
    public class GetListImageByParkingIdQuery : IRequest<ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int ParkingId { get; set; }
    }
}

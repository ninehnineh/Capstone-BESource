using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice
{
    public class GetAllParkingPriceQuery : IRequest<ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>>
    {
        public int ManagerId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }
}

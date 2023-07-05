using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating
{
    public class GetListParkingDesByRatingQuery : IRequest<ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}

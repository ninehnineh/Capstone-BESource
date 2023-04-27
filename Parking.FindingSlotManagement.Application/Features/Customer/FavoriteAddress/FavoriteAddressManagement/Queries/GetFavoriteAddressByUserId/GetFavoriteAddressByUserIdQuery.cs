using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressByUserId
{
    public class GetFavoriteAddressByUserIdQuery : IRequest<ServiceResponse<IEnumerable<GetFavoriteAddressByUserIdResponse>>>
    {
        public int UserId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}

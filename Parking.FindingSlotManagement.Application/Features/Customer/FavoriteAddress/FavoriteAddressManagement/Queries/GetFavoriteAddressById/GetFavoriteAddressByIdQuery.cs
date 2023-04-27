using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressById
{
    public class GetFavoriteAddressByIdQuery : IRequest<ServiceResponse<GetFavoriteAddressByIdResponse>>
    {
        public int FavoriteAddressId { get; set; }
    }
}

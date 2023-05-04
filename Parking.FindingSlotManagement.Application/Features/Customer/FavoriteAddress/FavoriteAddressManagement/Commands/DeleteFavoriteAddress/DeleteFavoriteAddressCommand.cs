using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.DeleteFavoriteAddress
{
    public class DeleteFavoriteAddressCommand : IRequest<ServiceResponse<string>>
    {
        public int FavoriteAddressId { get; set; }
    }
}

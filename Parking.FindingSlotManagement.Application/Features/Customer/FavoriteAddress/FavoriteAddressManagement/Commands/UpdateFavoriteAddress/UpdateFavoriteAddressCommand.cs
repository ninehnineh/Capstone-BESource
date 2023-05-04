using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.UpdateFavoriteAddress
{
    public class UpdateFavoriteAddressCommand : IRequest<ServiceResponse<string>>
    {
        public int FavoriteAddressId { get; set; }
        public string? TagName { get; set; }
        public string? Address { get; set; }
    }
}

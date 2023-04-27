using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress
{
    public class CreateNewFavoriteAddressCommand : IRequest<ServiceResponse<int>>
    {
        public string? TagName { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }
    }
}

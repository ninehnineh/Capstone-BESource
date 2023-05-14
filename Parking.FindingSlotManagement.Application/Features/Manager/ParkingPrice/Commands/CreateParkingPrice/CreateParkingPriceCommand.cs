using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice
{
    public class CreateParkingPriceCommand : IRequest<ServiceResponse<int>>
    {
        public string? ParkingPriceName { get; set; }
        public int BusinessId { get; set; }
    }
}

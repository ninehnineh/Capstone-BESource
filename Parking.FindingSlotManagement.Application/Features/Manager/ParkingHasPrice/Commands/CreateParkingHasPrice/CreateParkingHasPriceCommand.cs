using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice
{
    public class CreateParkingHasPriceCommand : IRequest<ServiceResponse<int>>
    {
        public int? ParkingId { get; set; }
        public int? ParkingPriceId { get; set; }
    }
}

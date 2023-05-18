using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.UpdateParkingHasPrice
{
    public class UpdateParkingHasPriceCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingHasPriceId { get; set; }
        public int ParkingPriceId { get; set; }

    }
}

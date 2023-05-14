using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice
{
    public class DisableOrEnableParkingPriceCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingPriceId { get; set; }

    }
}

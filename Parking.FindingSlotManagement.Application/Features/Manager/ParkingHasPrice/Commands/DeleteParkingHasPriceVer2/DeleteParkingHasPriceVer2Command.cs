using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPriceVer2
{
    public class DeleteParkingHasPriceVer2Command : IRequest<ServiceResponse<string>>
    {
        public int ParkingId { get; set; }
        public int ParkingPriceId { get; set; }
    }
}

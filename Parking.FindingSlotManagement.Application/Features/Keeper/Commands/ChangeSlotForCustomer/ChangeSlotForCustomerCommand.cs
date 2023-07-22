using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer
{
    public class ChangeSlotForCustomerCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingSlotId { get; set; }
/*        public int BookingIdOvertime { get; set; }*/
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetNumberOfBookedSlots
{
    public class GetNumberOfBookedSlotsQuery : IRequest<ServiceResponse<int>>
    {
    }
}

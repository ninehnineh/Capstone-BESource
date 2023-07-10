using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCard
{
    public class GetStatisticBaseOnCardQuery : IRequest<ServiceResponse<GetStatisticBaseOnCardResponse>>
    {
        public int ManagerId { get; set; }
    }
}

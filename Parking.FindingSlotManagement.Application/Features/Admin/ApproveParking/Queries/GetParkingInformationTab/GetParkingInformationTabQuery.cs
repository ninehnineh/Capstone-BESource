using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab
{
    public class GetParkingInformationTabQuery : IRequest<ServiceResponse<GetParkingInformationTabResponse>>
    {
        public int ParkingId { get; set; }
    }
}

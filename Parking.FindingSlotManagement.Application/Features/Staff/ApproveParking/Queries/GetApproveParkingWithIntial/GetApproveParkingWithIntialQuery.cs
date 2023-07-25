using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingWithIntial
{
    public class GetApproveParkingWithIntialQuery : IRequest<ServiceResponse<GetApproveParkingWithIntialResponse>>
    {
        public int ParkingId { get; set; }
    }
}

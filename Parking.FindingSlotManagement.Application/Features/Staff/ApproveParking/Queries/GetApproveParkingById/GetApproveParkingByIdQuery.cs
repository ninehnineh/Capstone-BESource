using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById
{
    public class GetApproveParkingByIdQuery : IRequest<ServiceResponse<GetApproveParkingByIdResponse>>
    {
        public int ApproveParkingId { get; set; }
    }
}

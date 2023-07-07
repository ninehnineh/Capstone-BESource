using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetFeeById
{
    public class GetFeeByIdQuery : IRequest<ServiceResponse<GetFeeByIdResponse>>
    {
        public int FeeId { get; set; }
    }
}

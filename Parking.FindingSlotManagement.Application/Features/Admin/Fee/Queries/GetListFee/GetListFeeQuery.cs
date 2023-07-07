using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetListFee
{
    public class GetListFeeQuery : IRequest<ServiceResponse<IEnumerable<GetListFeeResponse>>>
    {
    }
}

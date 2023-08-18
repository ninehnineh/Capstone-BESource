using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfBusinessAccount
{
    public class SumOfBusinessAccountQuery : IRequest<ServiceResponse<SumOfBusinessAccountResponse>>
    {
    }
}

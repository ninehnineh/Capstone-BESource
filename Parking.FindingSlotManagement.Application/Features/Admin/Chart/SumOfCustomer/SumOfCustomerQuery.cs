using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfCustomer
{
    public class SumOfCustomerQuery : IRequest<ServiceResponse<SumOfCustomerResponse>>
    {
    }
}

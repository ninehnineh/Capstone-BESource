using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.DeleteFee
{
    public class DeleteFeeCommand : IRequest<ServiceResponse<string>>
    {
        public int FeeId { get; set; }
    }
}

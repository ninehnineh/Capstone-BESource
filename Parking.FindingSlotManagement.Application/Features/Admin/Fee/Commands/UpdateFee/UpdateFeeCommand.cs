using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.UpdateFee
{
    public class UpdateFeeCommand : IRequest<ServiceResponse<string>>
    {
        public int FeeId { get; set; }
        public string? BusinessType { get; set; }
        public decimal Price { get; set; }
        public string? Name { get; set; }
    }
}

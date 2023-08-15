using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.CreateNewFee
{
    public class CreateNewFeeCommand : IRequest<ServiceResponse<int>>
    {
        public string? Name { get; set; }
        public string? BusinessType { get; set; }
        public decimal Price { get; set; }
        public string NumberOfParking { get; set; }

    }
}

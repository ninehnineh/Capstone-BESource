using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.DisableOrEnablePackagePrice
{
    public class DisableOrEnablePackagePriceCommand : IRequest<ServiceResponse<string>>
    {
        public int PackagePriceId { get; set; }
    }
}

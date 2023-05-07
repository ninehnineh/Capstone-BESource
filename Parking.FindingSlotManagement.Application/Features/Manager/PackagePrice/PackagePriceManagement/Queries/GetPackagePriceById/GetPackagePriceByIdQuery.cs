using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Queries.GetPackagePriceById
{
    public class GetPackagePriceByIdQuery : IRequest<ServiceResponse<GetPackagePriceByIdResponse>>
    {
        public int PackagePriceId { get; set; }
    }
}

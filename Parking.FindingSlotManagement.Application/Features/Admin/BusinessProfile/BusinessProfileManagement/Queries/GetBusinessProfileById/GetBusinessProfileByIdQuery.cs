using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileById
{
    public class GetBusinessProfileByIdQuery : IRequest<ServiceResponse<GetBusinessProfileByIdResponse>>
    {
        public int BusinessProfileId { get; set; }
    }
}

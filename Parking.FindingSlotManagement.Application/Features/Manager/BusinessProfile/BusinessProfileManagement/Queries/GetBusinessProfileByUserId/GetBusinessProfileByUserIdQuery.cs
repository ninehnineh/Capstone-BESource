using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileByUserId
{
    public class GetBusinessProfileByUserIdQuery : IRequest<ServiceResponse<GetBusinessProfileResponse>>
    {
        public int UserId { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetInforOfBusinessByManagerId
{
    public class GetInforOfBusinessByManagerIdQuery : IRequest<ServiceResponse<GetInforOfBusinessByManagerIdResponse>>
    {
        public int ManagerId { get; set; }
    }
}

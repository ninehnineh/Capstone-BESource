using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile
{
    public class GetListBusinessProfileQuery : IRequest<ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Commands.DeleteBusinessProfile
{
    public class DeleteBusinessProfileCommand : IRequest<ServiceResponse<string>>
    {
        public int BusinessProfileId { get; set; }
    }
}

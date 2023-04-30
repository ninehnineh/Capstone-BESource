using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Commands.CreateNewBusinessProfile
{
    public class CreateNewBusinessProfileCommand : IRequest<ServiceResponse<int>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? FrontIdentification { get; set; }
        public string? BackIdentification { get; set; }
        public string? BusinessLicense { get; set; }
        public int? UserId { get; set; }
    }
}

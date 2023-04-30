using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile
{
    public class GetListBusinessProfileResponse
    {
        public int BusinessProfileId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? FrontIdentification { get; set; }
        public string? BackIdentification { get; set; }
        public string? BusinessLicense { get; set; }
        public string? UserName { get; set; }
    }
}

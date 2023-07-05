using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByBusinessId
{
    public class GetVnPayByBusinessIdResponse
    {
        public int VnPayId { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public int? BusinessId { get; set; }
        public string? BusinessName { get; set; }
    }
}

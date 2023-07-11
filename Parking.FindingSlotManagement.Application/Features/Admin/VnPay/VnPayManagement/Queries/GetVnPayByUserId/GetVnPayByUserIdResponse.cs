using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByUserId
{
    public class GetVnPayByUserIdResponse
    {
        public int VnPayId { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}

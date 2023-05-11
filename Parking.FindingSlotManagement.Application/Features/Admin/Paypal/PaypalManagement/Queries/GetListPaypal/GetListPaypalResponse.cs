using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal
{
    public class GetListPaypalResponse
    {
        public int PayPalId { get; set; }
        public string? ClientId { get; set; }
        public string? SecretKey { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
    }
}

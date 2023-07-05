using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Wallet
{
    public class DepositTransaction
    {
        public decimal? TotalPrice { get; set; }
        public string OrderType { get; set; } = "Dịch vụ";
        public int? UserId { get; set; }
    }
}

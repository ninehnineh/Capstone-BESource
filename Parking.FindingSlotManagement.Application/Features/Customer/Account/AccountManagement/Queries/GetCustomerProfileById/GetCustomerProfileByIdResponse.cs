using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetCustomerProfileById
{
    public class GetCustomerProfileByIdResponse
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? IdCardNo { get; set; }
        public DateTime? IdCardDate { get; set; }
        public string? IdCardIssuedBy { get; set; }
        public string? Address { get; set; }
    }
}

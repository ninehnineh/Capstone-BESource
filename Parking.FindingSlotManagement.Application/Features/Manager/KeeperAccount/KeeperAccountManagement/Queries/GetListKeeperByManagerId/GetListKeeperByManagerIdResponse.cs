using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId
{
    public class GetListKeeperByManagerIdResponse
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public string? Gender { get; set; }
        public int? ParkingId { get; set; }
        public string? ParkingName { get; set; }
    }
}

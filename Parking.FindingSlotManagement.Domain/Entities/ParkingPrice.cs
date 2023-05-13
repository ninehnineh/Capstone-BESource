using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingPrice
    {
        public int ParkingPriceId { get; set; }
        public string? ParkingPriceName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TimeLine>? TimeLines { get; set; }
        public virtual ICollection<ParkingHasPrice>? ParkingHasPrices { get; set; }
    }
}

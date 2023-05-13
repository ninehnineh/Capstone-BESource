using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingHasPrice
    {
        public int ParkingHasPriceId { get; set; }
        public int? ParkingId { get; set; }
        public int? ParkingPriceId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual ParkingPrice? ParkingPrice { get; set; }  
    }
}

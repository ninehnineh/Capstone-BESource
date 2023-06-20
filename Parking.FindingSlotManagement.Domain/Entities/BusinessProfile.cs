using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class BusinessProfile
    {
        public int BusinessProfileId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? FrontIdentification { get; set; }
        public string? BackIdentification { get; set; }
        public string? BusinessLicense { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }

        public ICollection<FieldWorkImg>? FieldWorkImgs { get; set; }
        public ICollection<Parking> Parkings { get; set; }
        public virtual ICollection<VnPay> VnPays { get; set; }
        public ICollection<ParkingPrice> ParkingPrices { get; set; }

    }
}

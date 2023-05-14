using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class FieldWorkImg
    {
        public int FieldWorkImgId { get; set; }
        public string? ImgUrl { get; set; }

        public int BusinessProfileId { get; set; }
        public BusinessProfile? BusinessProfile { get; set; }
    }
}

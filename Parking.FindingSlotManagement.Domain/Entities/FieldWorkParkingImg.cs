using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class FieldWorkParkingImg
    {
        public int FieldWorkParkingImgId { get; set; }
        public string? Url { get; set; }

        public int? ApproveParkingId { get; set; }
        public ApproveParking? ApproveParking { get; set; }

    }
}

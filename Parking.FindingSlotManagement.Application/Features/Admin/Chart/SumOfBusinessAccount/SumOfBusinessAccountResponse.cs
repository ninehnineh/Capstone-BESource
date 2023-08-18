using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfBusinessAccount
{
    public class SumOfBusinessAccountResponse
    {
        public int NumberOfBusinessAccount { get; set; }
        public int NumberOfAccountUsingApp { get; set; }
        public int NumberOfParkingActive { get; set; }

    }
}

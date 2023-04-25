using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application
{
    public enum ResponseCode
    {
        OK = 200,
        Error = 500,
        UnAuthorize = 401,
        Forbidden = 403,
        BadRequest = 400
    }
}

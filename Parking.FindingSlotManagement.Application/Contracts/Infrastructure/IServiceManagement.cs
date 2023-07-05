using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Infrastructure
{
    public interface IServiceManagement
    {
        void SendEmail();
        void DeleteTimeSlotIn1Week();
        void GenerateMerchandise();
        void SyncData();
        void AddTimeSlotInFuture(int floorId);
    }
}

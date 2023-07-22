using Parking.FindingSlotManagement.Application.Models.PushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Infrastructure
{
    public interface IFireBaseMessageServices
    {
        Task<string> SendNotificationToWebAsync(PushNotificationWebModel pushNotificationModel);
        Task<string> SendNotificationToMobileAsync(PushNotificationMobileModel pushNotificationModel);

        void SendNotificationToWebAsyncV2(PushNotificationWebModel pushNotificationModel);
        void SendNotificationToMobileAsyncV2(PushNotificationMobileModel pushNotificationModel);
    }
}

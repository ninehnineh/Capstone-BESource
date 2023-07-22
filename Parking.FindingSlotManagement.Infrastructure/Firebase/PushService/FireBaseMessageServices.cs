using FirebaseAdmin.Messaging;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Firebase.PushService
{
    public class FireBaseMessageServices : IFireBaseMessageServices
    {
        public async Task<string> SendNotificationToMobileAsync(PushNotificationMobileModel pushNotificationMobileModel)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            // Construct the notification message
            var notification = new Notification
            {
                Title = pushNotificationMobileModel.Title,
                Body = pushNotificationMobileModel.Message
            };

            // Construct the message payload
            var messagePayload = new Message
            {
                Notification = notification,
                Token = pushNotificationMobileModel.TokenMobile
            };

            // Send the message
            var response = await messaging.SendAsync(messagePayload);
            return response;
        }

        public void SendNotificationToMobileAsyncV2(PushNotificationMobileModel pushNotificationMobileModel)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            // Construct the notification message
            var notification = new Notification
            {
                Title = pushNotificationMobileModel.Title,
                Body = pushNotificationMobileModel.Message
            };

            // Construct the message payload
            var messagePayload = new Message
            {
                Notification = notification,
                Token = pushNotificationMobileModel.TokenMobile
            };

            // Send the message
            var response = messaging.SendAsync(messagePayload);
        }

        public async Task<string> SendNotificationToWebAsync(PushNotificationWebModel pushNotificationModel)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = pushNotificationModel.Title,
                    Body = pushNotificationModel.Message
                },
                Webpush = new WebpushConfig()
                {
                    Notification = new WebpushNotification()
                    {
                        Icon = "https://cdn-icons-png.flaticon.com/512/147/147142.png",
                    },
                },
                Token = pushNotificationModel.TokenWeb,
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

        public void SendNotificationToWebAsyncV2(PushNotificationWebModel pushNotificationModel)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = pushNotificationModel.Title,
                    Body = pushNotificationModel.Message
                },
                Webpush = new WebpushConfig()
                {
                    Notification = new WebpushNotification()
                    {
                        Icon = "https://cdn-icons-png.flaticon.com/512/147/147142.png",
                    },
                },
                Token = pushNotificationModel.TokenWeb,
            };
            var response =  FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}

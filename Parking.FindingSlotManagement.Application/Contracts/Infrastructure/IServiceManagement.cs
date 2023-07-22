using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Infrastructure
{
    public interface IServiceManagement
    {
        void SendEmail(int entity);
        void DeleteTimeSlotIn1Week();
        void GenerateMerchandise();
        void SyncData();
        void AddTimeSlotInFuture(int floorId);
        void AutoCancelBookingWhenOverAllowTimeBooking(int bookingId);
        void AutoCancelBookingWhenOutOfEndTimeBooking(int bookingId);
        void ChargeMoneyFor1MonthUsingSystem(Fee fee, int bussinesId, int billId, User user);
        void CheckIfBookingIsLateOrNot(int bookingId, int parkingId, List<string> Token, User ManagerOfParking);
    }
}

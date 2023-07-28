namespace Parking.FindingSlotManagement.Domain.Enum
{
    public enum BookingStatus
    {
        Initial,
        Success,
        Check_In,
        Check_Out,
        Waiting_For_Payment,
        Payment_Successed,
        OverTime,
        Done,
        Cancel
    }
}

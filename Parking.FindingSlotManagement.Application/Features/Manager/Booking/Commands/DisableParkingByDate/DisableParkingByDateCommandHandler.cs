using Hangfire;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate;
/// Keeper/Manager story: set job để hệ thống tự động disable parking tại một ngày trong tương lai 
namespace Parking.FindingSlotManagement.Application.Features.Manager.Commands.DisableParkingByDate
{

    public class DisableParkingByDateCommandHandler : IRequestHandler<DisableParkingByDateCommand, ServiceResponse<string>>
    {
        // private readonly IParkingSlotRepository parkingSlotRepository;

        public DisableParkingByDateCommandHandler()
        {
            // this.parkingSlotRepository = parkingSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableParkingByDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var nowUTCDate = DateTime.UtcNow.Date;
                var nowDate = DateTime.Now.Date;
                var disableDate = request.DisableDate.Date;
                var disableDateTime = request.DisableDate;
                var disableDateFormated = disableDate.ToString("dd/MM/yyyy");
                var parkingId = request.ParkingId;
                var reason = request.Reason;
                var nextDate = nowUTCDate.AddDays(1);

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);
                // nhớ set rule cho disableDate, có cho phép đặt lịch disable 1 tháng hay 1 tuần hay 1 năm 
                if (disableDate.Month > nowDate.Month)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Chỉ có thể tắt bãi trong vòng một tháng trở lại"
                    };
                }
                if (disableDate == nextDate)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Ngày tắt bãi không hợp lệ, không thể tắt bãi tại {disableDateFormated}"
                    };
                }
                if (disableDate <= nowUTCDate)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Ngày tắt bãi không hợp lệ, không thể tắt bãi tại {disableDateFormated}"
                    };
                }

                var timeToCallJob = disableDateTime - nowUTCDate.AddHours(7);
                BackgroundJob.Schedule<IServiceManagement>(x => x.DisableParkingByDate(parkingId, disableDate), timeToCallJob);

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingSlotByDateCommandHandler: Message {ex.Message}");
            }
        }
    }
}
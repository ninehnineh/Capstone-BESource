using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CancelDisableScheduledParking
{
    public class CancelDisableScheduledParkingCommandHandler : IRequestHandler<CancelDisableScheduledParkingCommand, ServiceResponse<string>>
    {
        private readonly IHangfireRepository hangfireRepository;

        public CancelDisableScheduledParkingCommandHandler(IHangfireRepository hangfireRepository)
        {
            this.hangfireRepository = hangfireRepository;
        }
        public async Task<ServiceResponse<string>> Handle(CancelDisableScheduledParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                var disableDate = request.DisableDate;

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                var result = await hangfireRepository.DeleteScheduledJob(parkingId, disableDate);

                if (!result.Equals("Xóa thành công"))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Có lỗi xảy ra khi xóa lịch",
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Error at CancelDisableScheduledParkingCommandHandler: Message {ex.Message}");
            }
        }
    }
}
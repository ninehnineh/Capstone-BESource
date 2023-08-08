using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.EnableParking
{
    public class EnableParkingAtDateCommandHandler : IRequestHandler<EnableParkingAtDateCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository parkingRepository;
        private readonly IHangfireRepository hangfireRepository;
        public EnableParkingAtDateCommandHandler(IParkingRepository parkingRepository, IHangfireRepository hangfireRepository)
        {
            this.hangfireRepository = hangfireRepository;
            this.parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(EnableParkingAtDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                var disableDate = request.DisableDate.Date;

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                var hasRow = await parkingRepository.GetDisableParking(parkingId, disableDate);
                if (!hasRow)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không có thông tin dữ liệu",
                    };
                }
                else if (hasRow)
                {
                    await parkingRepository.EnableDisableParkingById(parkingId, disableDate);
                    await hangfireRepository.DeleteJob("DisableParkingByDate", parkingId, disableDate);
                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at EnableParkingCommandHandler: Message {ex.Message}");
            }
        }
    }
}
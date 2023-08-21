using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.EnableParkingAtCurrentTime
{
    public class EnableParkingAtCurrentTimeCommandHandler : IRequestHandler<EnableParkingAtCurrentTimeCommand, ServiceResponse<string>>
    {
        public EnableParkingAtCurrentTimeCommandHandler()
        {

        }
        public Task<ServiceResponse<string>> Handle(EnableParkingAtCurrentTimeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                var disableDate = request.DisableDate.Date;

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                

                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at EnableParkingAtCurrentTimeCommandHandler: Message {ex.Message}");
            }
        }
    }
}
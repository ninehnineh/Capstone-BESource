using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.EnableParkingSlot
{
    public class EnableParkingSlotCommandHandler : IRequestHandler<EnableParkingSlotCommand, ServiceResponse<string>>
    {
        private readonly IParkingSlotRepository parkingSlotRepository;

        public EnableParkingSlotCommandHandler(IParkingSlotRepository parkingSlotRepository)
        {
            this.parkingSlotRepository = parkingSlotRepository;
        }
        public async Task<ServiceResponse<string>> Handle(EnableParkingSlotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var slotId = request.ParkingSlotId;

                ArgumentNullException.ThrowIfNull(slotId);

                await parkingSlotRepository.EnableParkingSlot(slotId);
                
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true,
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at EnableParkingSlotCommandHandler: Message {ex.Message}");
            }
        }
    }
}
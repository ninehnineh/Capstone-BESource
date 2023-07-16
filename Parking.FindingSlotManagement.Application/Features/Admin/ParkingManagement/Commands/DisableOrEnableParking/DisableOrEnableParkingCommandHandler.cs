using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Commands.DisableOrEnableParking
{
    public class DisableOrEnableParkingCommandHandler : IRequestHandler<DisableOrEnableParkingCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository _parkingRepository;

        public DisableOrEnableParkingCommandHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(parkingExist.IsActive == true)
                {
                    parkingExist.IsActive = false;
                }
                else if(parkingExist.IsActive == false)
                {
                    parkingExist.IsActive = true;
                }
                await _parkingRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204

                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

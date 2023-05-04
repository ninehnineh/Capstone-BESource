using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.DisableOrEnableParking
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
                var checkExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                if(checkExist.IsActive == true)
                {
                    checkExist.IsActive = false;
                }
                else if(checkExist.IsActive == false)
                {
                    checkExist.IsActive = true;
                }
                await _parkingRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

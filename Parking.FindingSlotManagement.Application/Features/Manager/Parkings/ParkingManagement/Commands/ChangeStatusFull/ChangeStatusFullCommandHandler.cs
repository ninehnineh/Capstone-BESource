using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.ChangeStatusFull
{
    public class ChangeStatusFullCommandHandler : IRequestHandler<ChangeStatusFullCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository _parkingRepository;

        public ChangeStatusFullCommandHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(ChangeStatusFullCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(parkingExist.IsActive == false)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi giữ xe đã bị vô hiệu hóa.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                if(parkingExist.IsFull == false)
                {
                    parkingExist.IsFull = true;
                }
                else if(parkingExist.IsFull == true)
                {
                    parkingExist.IsFull = false;
                }
                await _parkingRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

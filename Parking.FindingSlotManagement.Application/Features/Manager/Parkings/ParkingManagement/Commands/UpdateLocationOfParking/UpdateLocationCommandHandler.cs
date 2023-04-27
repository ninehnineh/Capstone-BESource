using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateLocationOfParking
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository _parkingRepository;

        public UpdateLocationCommandHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                checkExist.Latitude = request.Latitude;
                checkExist.Longitude = request.Longitude;
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

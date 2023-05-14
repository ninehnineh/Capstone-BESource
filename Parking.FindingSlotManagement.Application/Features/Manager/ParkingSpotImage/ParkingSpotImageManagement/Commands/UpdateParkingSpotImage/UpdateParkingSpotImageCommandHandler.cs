using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.UpdateParkingSpotImage
{
    public class UpdateParkingSpotImageCommandHandler : IRequestHandler<UpdateParkingSpotImageCommand, ServiceResponse<string>>
    {
        private readonly IParkingSpotImageRepository _parkingSpotImageRepository;
        private readonly IParkingRepository _parkingRepository;

        public UpdateParkingSpotImageCommandHandler(IParkingSpotImageRepository parkingSpotImageRepository, IParkingRepository parkingRepository)
        {
            _parkingSpotImageRepository = parkingSpotImageRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateParkingSpotImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkParkingImageExist = await _parkingSpotImageRepository.GetById(request.ParkingSpotImageId);
                if(checkParkingImageExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.ImgPath))
                {
                    checkParkingImageExist.ImgPath = request.ImgPath;
                }
                if(!string.IsNullOrEmpty(request.ParkingId.ToString()))
                {
                    var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                    if(checkParkingExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy bãi giữ xe.",
                            Success = true,
                            StatusCode = 200
                        };
                    }
                    checkParkingImageExist.ParkingId = request.ParkingId;
                }
                await _parkingSpotImageRepository.Update(checkParkingImageExist);
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

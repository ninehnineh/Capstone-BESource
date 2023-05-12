using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.DeleteParkingSpotImage
{
    public class DeleteParkingSpotImageCommandHandler : IRequestHandler<DeleteParkingSpotImageCommand, ServiceResponse<string>>
    {
        private readonly IParkingSpotImageRepository _parkingSpotImageRepository;

        public DeleteParkingSpotImageCommandHandler(IParkingSpotImageRepository parkingSpotImageRepository)
        {
            _parkingSpotImageRepository = parkingSpotImageRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteParkingSpotImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _parkingSpotImageRepository.GetById(request.ParkingSpotImageId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                await _parkingSpotImageRepository.Delete(checkExist);
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

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPriceVer2
{
    public class DeleteParkingHasPriceVer2CommandHandler : IRequestHandler<DeleteParkingHasPriceVer2Command, ServiceResponse<string>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IParkingRepository _parkingRepository;

        public DeleteParkingHasPriceVer2CommandHandler(IParkingHasPriceRepository parkingHasPriceRepository, IParkingPriceRepository parkingPriceRepository, IParkingRepository parkingRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteParkingHasPriceVer2Command request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var parkingPriceExist = await _parkingPriceRepository.GetById(request.ParkingPriceId);
                if (parkingPriceExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var parkingHasPriceExist = await _parkingHasPriceRepository.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.ParkingId == request.ParkingId, null, false);
                if(parkingHasPriceExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy yêu cầu áp dụng gói.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                await _parkingHasPriceRepository.Delete(parkingHasPriceExist);
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

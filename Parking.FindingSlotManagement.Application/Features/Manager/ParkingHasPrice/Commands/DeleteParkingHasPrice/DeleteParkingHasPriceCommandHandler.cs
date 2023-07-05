using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPrice
{
    public class DeleteParkingHasPriceCommandHandler : IRequestHandler<DeleteParkingHasPriceCommand, ServiceResponse<string>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;

        public DeleteParkingHasPriceCommandHandler(IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteParkingHasPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingHasPrice = await _parkingHasPriceRepository.GetById(request.ParkingHasPriceId);
                if (parkingHasPrice == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tồn tại",
                        StatusCode = 404,
                        Success = false
                    };
                }

                await _parkingHasPriceRepository.Delete(parkingHasPrice);
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

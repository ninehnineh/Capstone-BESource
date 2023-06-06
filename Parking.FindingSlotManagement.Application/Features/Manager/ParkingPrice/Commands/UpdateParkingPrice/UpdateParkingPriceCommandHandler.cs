/*using MediatR;
using NuGet.Protocol.Plugins;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.UpdateParkingPrice
{
    public class UpdateParkingPriceCommandHandler : IRequestHandler<UpdateParkingPriceCommand, ServiceResponse<string>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITrafficRepository _trafficRepository;

        public UpdateParkingPriceCommandHandler(IParkingPriceRepository parkingPriceRepository, IUserRepository userRepository, ITrafficRepository trafficRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _userRepository = userRepository;
            _trafficRepository = trafficRepository;
        }

        public async Task<ServiceResponse<string>> Handle(UpdateParkingPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingPrice = await _parkingPriceRepository
                    .GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false);
                if(!string.IsNullOrEmpty(request.ParkingPriceName))
                {
                    parkingPrice.ParkingPriceName = request.ParkingPriceName;
                }
                if (!string.IsNullOrEmpty(request.BusinessId.ToString()))
                {
                    var checkBusinessExist = await _userRepository.GetById(request.BusinessId);
                    if(checkBusinessExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy tài khoản doanh nghiệp.",
                            StatusCode = 200,
                            Success = true
                        };
                    }
                    if(checkBusinessExist.RoleId != 1)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Tài khoản không phải doanh nghiệp.",
                            StatusCode = 400,
                            Success = false
                        };
                    }
                    parkingPrice.UserId = (int)request.BusinessId;
                }
                if (!string.IsNullOrEmpty(request.TrafficId.ToString()))
                {
                    var checkTrafficExist = await _trafficRepository.GetById(request.TrafficId);
                    if(checkTrafficExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy phương tiện.",
                            StatusCode = 200,
                            Success = true
                        };
                    }
                    parkingPrice.TrafficId = request.TrafficId;
                }
                await _parkingPriceRepository.Update(parkingPrice);

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
*/
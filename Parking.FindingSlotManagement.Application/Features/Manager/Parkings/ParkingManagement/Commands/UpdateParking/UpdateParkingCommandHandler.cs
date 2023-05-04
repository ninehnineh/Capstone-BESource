using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateParking
{
    public class UpdateParkingCommandHandler : IRequestHandler<UpdateParkingCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository _parkingRepository;

        public UpdateParkingCommandHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateParkingCommand request, CancellationToken cancellationToken)
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
                if(!string.IsNullOrEmpty(request.Name))
                {
                    var checkExistByName = await _parkingRepository.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true);
                    if(checkExistByName != null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Tên bãi đã tồn tại. Vui lòng nhập tên bãi khác.",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    checkExist.Name = request.Name;
                }
                if(!string.IsNullOrEmpty(request.Address))
                {
                    checkExist.Address = request.Address;
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    checkExist.Description = request.Description;
                }
                if (!string.IsNullOrEmpty(request.MotoSpot.ToString()))
                {
                    if(request.MotoSpot < 0)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "{Số slot xe máy} phải lớn hơn bằng 0",
                            Success = false,
                            StatusCode = 400,
                            Count = 0
                        };
                    }
                    checkExist.MotoSpot = request.MotoSpot;
                }
                if (!string.IsNullOrEmpty(request.CarSpot.ToString()))
                {
                    if (request.CarSpot < 0)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "{Số slot xe ô tô} phải lớn hơn bằng 0",
                            Success = false,
                            StatusCode = 400,
                            Count = 0
                        };
                    }
                    checkExist.CarSpot = request.CarSpot;
                }
                if (!string.IsNullOrEmpty(request.IsPrepayment.ToString()))
                {
                    checkExist.IsPrepayment = request.IsPrepayment;
                }
                if (!string.IsNullOrEmpty(request.IsOvernight.ToString()))
                {
                    checkExist.IsOvernight = request.IsOvernight;
                }
                await _parkingRepository.Update(checkExist);
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

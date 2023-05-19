/*using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.UpdatePackagePrice
{
    public class UpdatePackagePriceCommandHandler : IRequestHandler<UpdatePackagePriceCommand, ServiceResponse<string>>
    {
        private readonly IPackagePriceRepository _packagePriceRepository;
        private readonly ITrafficRepository _trafficRepository;

        public UpdatePackagePriceCommandHandler(IPackagePriceRepository packagePriceRepository, ITrafficRepository trafficRepository)
        {
            _packagePriceRepository = packagePriceRepository;
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdatePackagePriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkPackagePriceExist = await _packagePriceRepository.GetById(request.PackagePriceId);
                if(checkPackagePriceExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))

                {
                    checkPackagePriceExist.Name = request.Name;
                }
                if (!string.IsNullOrEmpty(request.Price.ToString()))
                {
                    checkPackagePriceExist.Price = request.Price;
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    checkPackagePriceExist.Description = request.Description;
                }
                if (!string.IsNullOrEmpty(request.StartTime.ToString()))
                {
                    if (request.StartTime.Value < DateTime.UtcNow.Date)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.",
                            StatusCode = 400,
                            Success = false,
                        };
                    }
                    checkPackagePriceExist.StartTime = request.StartTime;
                }
                if (!string.IsNullOrEmpty(request.EndTime.ToString()))
                {
                    if (request.EndTime > DateTime.UtcNow.Date.AddDays(2))
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.",
                            StatusCode = 400,
                            Success = false,
                        };
                    }
                    if (request.EndTime.Value.TimeOfDay < request.StartTime.Value.TimeOfDay)
                    {
                        request.EndTime = request.StartTime.Value.AddDays(1).Date
                            .AddHours(request.EndTime.Value.Hour)
                            .AddMinutes(request.EndTime.Value.Minute)
                            .AddSeconds(request.EndTime.Value.Second);
                    }
                    checkPackagePriceExist.EndTime = request.EndTime;
                }
                if(!string.IsNullOrEmpty(request.IsExtrafee.ToString()))
                {
                    if(request.IsExtrafee == false)
                    {
                        checkPackagePriceExist.ExtraFee = null;
                        checkPackagePriceExist.PenaltyPrice = null;
                        checkPackagePriceExist.IsExtrafee = request.IsExtrafee;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.ExtraFee.ToString()))
                        {
                            checkPackagePriceExist.ExtraFee = request.ExtraFee;
                        }
                        if (!string.IsNullOrEmpty(request.ExtraTimeStep.ToString()))
                        {
                            checkPackagePriceExist.ExtraTimeStep = request.ExtraTimeStep;
                        }
                        checkPackagePriceExist.IsExtrafee = request.IsExtrafee;
                    }
                }
                else
                {
                    if(!string.IsNullOrEmpty(request.ExtraFee.ToString()))
                    {
                        checkPackagePriceExist.ExtraFee = request.ExtraFee;
                    }
                    if(!string.IsNullOrEmpty(request.ExtraTimeStep.ToString()))
                    {
                        checkPackagePriceExist.ExtraTimeStep = request.ExtraTimeStep;
                    }
                }

                if(!string.IsNullOrEmpty(request.HasPenaltyPrice.ToString()))
                {
                    if(request.HasPenaltyPrice == false)
                    {
                        checkPackagePriceExist.PenaltyPrice = null;
                        checkPackagePriceExist.PenaltyPriceStepTime = null;
                        checkPackagePriceExist.HasPenaltyPrice = request.HasPenaltyPrice;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.PenaltyPrice.ToString()))
                        {
                            checkPackagePriceExist.PenaltyPrice = request.PenaltyPrice;
                        }
                        if (!string.IsNullOrEmpty(request.PenaltyPriceStepTime.ToString()))
                        {
                            checkPackagePriceExist.PenaltyPriceStepTime = request.PenaltyPriceStepTime;
                        }
                        checkPackagePriceExist.HasPenaltyPrice = request.HasPenaltyPrice;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.PenaltyPrice.ToString()))
                    {
                        checkPackagePriceExist.PenaltyPrice = request.PenaltyPrice;
                    }
                    if (!string.IsNullOrEmpty(request.PenaltyPriceStepTime.ToString()))
                    {
                        checkPackagePriceExist.PenaltyPriceStepTime = request.PenaltyPriceStepTime;
                    }
                }
                if(!string.IsNullOrEmpty(request.TrafficId.ToString()))
                {
                    var checkTrafficExist = await _trafficRepository.GetById(request.TrafficId);
                    if(checkTrafficExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy phương tiện.",
                            Success = true,
                            StatusCode = 200
                        };
                    }
                    checkPackagePriceExist.TrafficId = request.TrafficId;
                }
                await _packagePriceRepository.Update(checkPackagePriceExist);
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
*/
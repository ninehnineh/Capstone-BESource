using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.CreateNewPackagePrice
{
    public class CreateNewPackagePriceCommandHandler : IRequestHandler<CreateNewPackagePriceCommand, ServiceResponse<int>>
    {
        private readonly IPackagePriceRepository _packagePriceRepository;
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public CreateNewPackagePriceCommandHandler(IPackagePriceRepository packagePriceRepository, ITrafficRepository trafficRepository)
        {
            _packagePriceRepository = packagePriceRepository;
            _trafficRepository = trafficRepository;
        }

        public async Task<ServiceResponse<int>> Handle(CreateNewPackagePriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExistTraffic = await _trafficRepository.GetById(request.TrafficId);
                if(checkExistTraffic == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy phương tiện.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                
                if (request.StartTime.Value < DateTime.UtcNow.Date)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.",
                        StatusCode = 400,
                        Success = false,
                    };
                }
                if (request.EndTime > DateTime.UtcNow.Date.AddDays(2))
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.",
                        StatusCode = 400,
                        Success = false,
                    };
                }
                if(request.EndTime.Value.TimeOfDay < request.StartTime.Value.TimeOfDay)
                {
                    request.EndTime = request.StartTime.Value.AddDays(1).Date
                        .AddHours(request.EndTime.Value.Hour)
                        .AddMinutes(request.EndTime.Value.Minute)
                        .AddSeconds(request.EndTime.Value.Second);
                }
                if (request.IsExtrafee == false)
                {
                    request.ExtraFee = null;
                    request.PenaltyPrice = null;
                }
                if (request.HasPenaltyPrice == false)
                {
                    request.PenaltyPrice = null;
                    request.PenaltyPriceStepTime = null;
                }
                var _mapper = config.CreateMapper();
                var packagePriceEntity = _mapper.Map<Domain.Entities.TimeLine>(request);
                packagePriceEntity.IsActive = false;
                await _packagePriceRepository.Insert(packagePriceEntity);
                return new ServiceResponse<int>
                {
                    Data = packagePriceEntity.TimeLineId,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice
{
    public class CreateParkingHasPriceCommandHandler : IRequestHandler<CreateParkingHasPriceCommand, ServiceResponse<int>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly ITimelineRepository _timelineRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateParkingHasPriceCommandHandler(IParkingHasPriceRepository parkingHasPriceRepository
            , IParkingRepository parkingRepository, IParkingPriceRepository parkingPriceRepository, ITimelineRepository timelineRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingRepository = parkingRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _timelineRepository = timelineRepository;
        }

        public async Task<ServiceResponse<int>> Handle(CreateParkingHasPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId!);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                List<Expression<Func<Domain.Entities.ParkingPrice, object>>> includes = new List<Expression<Func<Domain.Entities.ParkingPrice, object>>>
                {
                    x => x.TimeLines
                };
                var checkParkingPriceExist = await _parkingPriceRepository.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, includes, true);
                if(checkParkingPriceExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkParkingExist.IsActive == false)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe không khả dụng.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                if(checkParkingPriceExist.IsActive == false)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Gói không khả dụng.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                //check traffic of the parking and the timeline has match
                if(checkParkingExist.CarSpot == 0 && checkParkingPriceExist.TimeLines.FirstOrDefault().TrafficId != 1)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe không hổ trợ xe hơi nên áp dụng gói không phù hợp.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                if(checkParkingExist.MotoSpot == 0 && checkParkingPriceExist.TimeLines.FirstOrDefault().TrafficId != 2)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe không hổ trợ xe mô tô nên áp dụng gói không phù hợp.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                //check overnight == true and the timeline will have 24 hours
                List<TimeSpan> lstTime = new List<TimeSpan>();
                var lstTimline = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, null, true);
                if (lstTimline.Count() == 0)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Gói chưa có khung giờ, vui lòng tạo mới khung trước khi áp dụng gói vào bãi giữ xe.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                foreach (var item in lstTimline)
                {
                    var result = item.EndTime - item.StartTime;
                    lstTime.Add((TimeSpan)result);
                }
                TimeSpan sumOfTime = new TimeSpan();
                foreach (var item in lstTime)
                {
                    sumOfTime += item;
                }
                if(checkParkingExist.IsOvernight == true && sumOfTime.TotalHours != 24)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Gói không hợp lệ do bãi có áp dụng giữ 24h nên tổng số giờ của gói phải trong 24h.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var _mapper = config.CreateMapper();
                var entityDto = _mapper.Map<Domain.Entities.ParkingHasPrice>(request);
                await _parkingHasPriceRepository.Insert(entityDto);
                return new ServiceResponse<int>
                {
                    Data = entityDto.ParkingHasPriceId,
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

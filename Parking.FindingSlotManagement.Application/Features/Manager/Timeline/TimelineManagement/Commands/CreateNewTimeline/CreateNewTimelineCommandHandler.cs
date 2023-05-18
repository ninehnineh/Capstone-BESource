using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline
{
    public class CreateNewTimelineCommandHandler : IRequestHandler<CreateNewTimelineCommand, ServiceResponse<int>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly ITrafficRepository _trafficRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewTimelineCommandHandler(ITimelineRepository timelineRepository, ITrafficRepository trafficRepository, IParkingPriceRepository parkingPriceRepository)
        {
            _timelineRepository = timelineRepository;
            _trafficRepository = trafficRepository;
            _parkingPriceRepository = parkingPriceRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewTimelineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExistTraffic = await _trafficRepository.GetById(request.TrafficId);
                if (checkExistTraffic == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy phương tiện.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var lstTimeline = await _timelineRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingPriceId == request.ParkingPriceId);
                if(lstTimeline.Count() > 0)
                {
                    var firstTimeLine = lstTimeline.FirstOrDefault();
                    if(firstTimeLine.TrafficId != request.TrafficId)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Phương tiện của khung giời không trùng với phương tiện của gói.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                }
                var checkParkingPriceExist = await _parkingPriceRepository.GetById(request.ParkingPriceId);
                if(checkParkingPriceExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy gói.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var lstTimelineHasExist = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, x => x.TimeLineId, true);
                if(lstTimelineHasExist.Count() > 0)
                {
                    var lastEntity = lstTimelineHasExist.FirstOrDefault();
                    var goi_cu_end = lastEntity.EndTime;
                    var goi_cu_start = lastEntity.StartTime;
                    var gói_đang_định_dùng_start = request.StartTime;
                    var gói_đang_định_dùng_end = request.EndTime;
                    //qua ngay hom sau
                    if (gói_đang_định_dùng_end.Value.Date > gói_đang_định_dùng_start.Value.Date)
                    {
                        if (gói_đang_định_dùng_start.Value.TimeOfDay < goi_cu_end.Value.TimeOfDay)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Gói không hợp lệ",
                                StatusCode = 400,
                                Success = false,
                            };
                        }
                    }
                    else
                    {
                        if (gói_đang_định_dùng_start.Value.TimeOfDay < goi_cu_end.Value.TimeOfDay)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Gói không hợp lệ",
                                StatusCode = 400,
                                Success = false,
                            };
                        }
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
                    if (request.EndTime.Value.TimeOfDay < request.StartTime.Value.TimeOfDay)
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
                    var _mapper2 = config.CreateMapper();
                    var timeLineEntity2 = _mapper2.Map<TimeLine>(request);
                    timeLineEntity2.IsActive = true;
                    await _timelineRepository.Insert(timeLineEntity2);
                    return new ServiceResponse<int>
                    {
                        Message = "Thành công",
                        Data = timeLineEntity2.TimeLineId,
                        Success = true,
                        StatusCode = 201
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
                if (request.EndTime.Value.TimeOfDay < request.StartTime.Value.TimeOfDay)
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
                var timeLineEntity = _mapper.Map<TimeLine>(request);
                timeLineEntity.IsActive = true;
                await _timelineRepository.Insert(timeLineEntity);
                return new ServiceResponse<int>
                {
                    Message = "Thành công",
                    Data = timeLineEntity.TimeLineId,
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

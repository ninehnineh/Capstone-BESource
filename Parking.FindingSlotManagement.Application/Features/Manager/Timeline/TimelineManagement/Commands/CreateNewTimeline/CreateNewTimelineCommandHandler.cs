using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline
{
    public class CreateNewTimelineCommandHandler : IRequestHandler<CreateNewTimelineCommand, ServiceResponse<int>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewTimelineCommandHandler(ITimelineRepository timelineRepository, IParkingPriceRepository parkingPriceRepository)
        {
            _timelineRepository = timelineRepository;
            _parkingPriceRepository = parkingPriceRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewTimelineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var checkParkingPriceExist = await _parkingPriceRepository.GetById(request.ParkingPriceId);
                if (checkParkingPriceExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy gói.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                if(checkParkingPriceExist.IsWholeDay == true)
                {
                    var timeLineHasWholeDayExist = await _timelineRepository.GetItemWithCondition(x => x.ParkingPriceId == checkParkingPriceExist.ParkingPriceId);
                    if(timeLineHasWholeDayExist != null)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Bạn không thể tạo mới, do đã có timeline tồn tại rồi.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                    if (checkParkingPriceExist.IsExtrafee == false)
                    {
                        request.ExtraFee = null;
                    }

                    var entityMapper = new CreateNewTimelineCommandMapper
                    {
                        Name = request.Name,
                        Price = request.Price,
                        Description = request.Description,
                        StartTime = null,
                        EndTime = null,
                        ExtraFee = request.ExtraFee,
                        ParkingPriceId = request.ParkingPriceId
                    };
                    var _mapper = config.CreateMapper();
                    var timeLineEntity = _mapper.Map<TimeLine>(entityMapper);
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
                else
                {
                    /*if (request.EndTime < request.StartTime)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Giờ kết thúc phải lớn hơn giờ bắt đầu.",
                        StatusCode = 400,
                        Success = false
                    };
                }*/
                    var lstTimelineHasExist = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, x => x.TimeLineId, true);
                    if (lstTimelineHasExist.Count() > 0)
                    {
                        var TimeFirst = lstTimelineHasExist.LastOrDefault();
                        var TimeLast = lstTimelineHasExist.FirstOrDefault();
                        if (TimeFirst.StartTime == TimeLast.EndTime)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Không thể tạo mới khung giờ. Do các khung giờ trong gói đã đủ 24 tiếng. Bạn chỉ có thể tạo mới khi xóa các gói trước đó hoặc tạo mới 1 gói khác sau đó bạn có thể tạo mới khung giờ của gói đó.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                    }
                    var a = TimeSpan.Parse(request.EndTime) - TimeSpan.Parse(request.StartTime);
                    if (a.TotalHours < 1 && a.TotalHours >= 0)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Giờ kết thúc phải lớn hơn giờ bắt đầu 1 tiếng.",
                            StatusCode = 400,
                            Success = false
                        };
                    }
                    if (lstTimelineHasExist.Count() > 0)
                    {
                        var lastEntity = lstTimelineHasExist.FirstOrDefault();
                        var goi_cu_end = lastEntity.EndTime;
                        var goi_cu_start = lastEntity.StartTime;
                        var gói_đang_định_dùng_start = request.StartTime;
                        var gói_đang_định_dùng_end = request.EndTime;
                        if (TimeSpan.Parse(gói_đang_định_dùng_start) < goi_cu_end)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Gói không hợp lệ.",
                                StatusCode = 400,
                                Success = false,
                            };
                        }
                        /*if (request.StartTime.Value < DateTime.UtcNow.Date)
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
                        }*/
                        /*if (request.EndTime.Value.TimeOfDay < request.StartTime.Value.TimeOfDay)
                        {
                            request.EndTime = request.StartTime.Value.AddDays(1).Date
                                .AddHours(request.EndTime.Value.Hour)
                                .AddMinutes(request.EndTime.Value.Minute)
                                .AddSeconds(request.EndTime.Value.Second);
                        }*/
                        if (TimeSpan.Parse(request.EndTime) < TimeSpan.Parse(request.StartTime))
                        {
                            /*var resEndTime = TimeSpan.Parse(request.StartTime) + (TimeSpan.Parse(request.StartTime) - TimeSpan.Parse(request.EndTime));
                            request.EndTime = resEndTime.ToString();*/
                            var TimeTomorow = TimeSpan.Parse(request.EndTime) - TimeSpan.Parse("00:00:00");
                            var resEndTime = TimeSpan.FromHours(24) + TimeTomorow;
                            TimeSpan timeSpan = TimeSpan.ParseExact(resEndTime.ToString(), @"d\.hh\:mm\:ss", CultureInfo.InvariantCulture);
                            request.EndTime = timeSpan.ToString(@"hh\:mm\:ss");
                        }
                        if (checkParkingPriceExist.IsExtrafee == false)
                        {
                            request.ExtraFee = null;
                        }

                        var entityMapper2 = new CreateNewTimelineCommandMapper
                        {
                            Name = request.Name,
                            Price = request.Price,
                            Description = request.Description,
                            StartTime = TimeSpan.Parse(request.StartTime),
                            EndTime = TimeSpan.Parse(request.EndTime),
                            ExtraFee = request.ExtraFee,
                            ParkingPriceId = request.ParkingPriceId
                        };
                        var _mapper2 = config.CreateMapper();
                        var timeLineEntity2 = _mapper2.Map<TimeLine>(entityMapper2);
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
                    /*if (request.StartTime.Value < DateTime.UtcNow.Date)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.",
                            StatusCode = 400,
                            Success = false,
                        };
                    }
                    if (request.EndTime > request.StartTime.Value.AddDays(1))
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
                    }*/
                    if (checkParkingPriceExist.IsExtrafee == false)
                    {
                        request.ExtraFee = null;
                    }

                    var entityMapper = new CreateNewTimelineCommandMapper
                    {
                        Name = request.Name,
                        Price = request.Price,
                        Description = request.Description,
                        StartTime = TimeSpan.Parse(request.StartTime),
                        EndTime = TimeSpan.Parse(request.EndTime),
                        ExtraFee = request.ExtraFee,
                        ParkingPriceId = request.ParkingPriceId
                    };
                    var _mapper = config.CreateMapper();
                    var timeLineEntity = _mapper.Map<TimeLine>(entityMapper);
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

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

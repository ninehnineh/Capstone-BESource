using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandHandler : IRequestHandler<UpdateTimelineCommand, ServiceResponse<string>>
    {
        private readonly ITimelineRepository _timelineRepository;
        public UpdateTimelineCommandHandler(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                TimeSpan res_Start = new();
                TimeSpan res_End = new();
                var checkTimelineExist = await _timelineRepository.GetById(request.TimeLineId);
                if (checkTimelineExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy khung giờ.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (request.StartTime != null || request.EndTime != null || request.StartTime != null && request.EndTime != null)
                {
                    res_Start = TimeSpan.Parse(request.StartTime);
                    res_End = TimeSpan.Parse(request.EndTime);
                }
                if (!string.IsNullOrEmpty(request.Name))

                {
                    checkTimelineExist.Name = request.Name;
                }
                if (!string.IsNullOrEmpty(request.Price.ToString()))
                {
                    checkTimelineExist.Price = (int)request.Price;
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    checkTimelineExist.Description = request.Description;
                }
                if (request.StartTime != null || request.EndTime != null || request.StartTime != null && request.EndTime != null)
                {

                    if (string.IsNullOrEmpty(request.StartTime.ToString()) == null)
                    {
                        res_Start = (TimeSpan)checkTimelineExist.StartTime;
                    }
                    if (string.IsNullOrEmpty(request.EndTime.ToString()) == null)
                    {
                        res_End = (TimeSpan)checkTimelineExist.EndTime;
                    }
/*                    if (res_Start < DateTime.UtcNow.Date)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.",
                            StatusCode = 400,
                            Success = false,
                        };
                    }
                    if (res_End > DateTime.UtcNow.Date.AddDays(2))
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.",
                            StatusCode = 400,
                            Success = false,
                        };
                    }
                    if (res_End.Value.TimeOfDay < res_Start.Value.TimeOfDay)
                    {
                        res_End = res_Start.Value.AddDays(1).Date
                            .AddHours(res_End.Value.Hour)
                            .AddMinutes(res_End.Value.Minute)
                            .AddSeconds(res_End.Value.Second);
                    }*/
                    var lstTimelineHasExist = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == checkTimelineExist.ParkingPriceId && x.IsActive == true && x.TimeLineId != checkTimelineExist.TimeLineId, null, x => x.TimeLineId, true);
                    var lastEntity = lstTimelineHasExist.FirstOrDefault();
                    if (lstTimelineHasExist.Count() > 0)
                    {
                        var goi_cu_end = lastEntity.EndTime;
                        var goi_cu_start = lastEntity.StartTime;
                        var gói_đang_định_dùng_start = res_Start;
                        var gói_đang_định_dùng_end = res_End;
                        //qua ngay hom sau
                        if (gói_đang_định_dùng_end < gói_đang_định_dùng_start)
                        {
                            if (gói_đang_định_dùng_start < goi_cu_end || gói_đang_định_dùng_end > goi_cu_start)
                            {
                                return new ServiceResponse<string>
                                {
                                    Message = "Gói không hợp lệ",
                                    StatusCode = 400,
                                    Success = false,
                                };
                            }
                        }
                        else
                        {
                            if (gói_đang_định_dùng_start < goi_cu_end || gói_đang_định_dùng_end < goi_cu_start)
                            {
                                return new ServiceResponse<string>
                                {
                                    Message = "Gói không hợp lệ",
                                    StatusCode = 400,
                                    Success = false,
                                };
                            }
                        }

                        checkTimelineExist.StartTime = res_Start;
                        checkTimelineExist.EndTime = res_End;
                   }
                   else
                   {
//                        /*if (res_Start.Value < DateTime.UtcNow.Date)
//                        {
//                            return new ServiceResponse<string>
//                            {
//                                Message = "Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.",
//                                StatusCode = 400,
//                                Success = false,
//                            };
//                        }
//                        if (res_End > DateTime.UtcNow.Date.AddDays(2))
//                        {
//                            return new ServiceResponse<string>
//                            {
//                                Message = "Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.",
//                                StatusCode = 400,
//                                Success = false,
//                            };
//                        }
//                        if (res_End.Value.TimeOfDay < res_Start.Value.TimeOfDay)
//                        {
//                            res_End = res_Start.Value.AddDays(1).Date
//                                .AddHours(res_End.Value.Hour)
//                                .AddMinutes(res_End.Value.Minute)
//                                .AddSeconds(res_End.Value.Second);
//                        }*/
                        checkTimelineExist.StartTime = res_Start;
                       checkTimelineExist.EndTime = res_End;
                 }
                }
                /*if (!string.IsNullOrEmpty(request.IsExtrafee.ToString()))
                {
                    if (request.IsExtrafee == false)
                    {
                        checkTimelineExist.ExtraFee = null;
                        checkTimelineExist.PenaltyPrice = null;
                        checkTimelineExist.IsExtrafee = request.IsExtrafee;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.ExtraFee.ToString()))
                        {
                            checkTimelineExist.ExtraFee = request.ExtraFee;
                        }
                        if (!string.IsNullOrEmpty(request.ExtraTimeStep.ToString()))
                        {
                            checkTimelineExist.ExtraTimeStep = request.ExtraTimeStep;
                        }
                        checkTimelineExist.IsExtrafee = request.IsExtrafee;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.ExtraFee.ToString()))
                    {
                        checkTimelineExist.ExtraFee = request.ExtraFee;
                    }
                    if (!string.IsNullOrEmpty(request.ExtraTimeStep.ToString()))
                    {
                        checkTimelineExist.ExtraTimeStep = request.ExtraTimeStep;
                    }
                }*/

                /*if (!string.IsNullOrEmpty(request.HasPenaltyPrice.ToString()))
                {
                    if (request.HasPenaltyPrice == false)
                    {
                        checkTimelineExist.PenaltyPrice = null;
                        checkTimelineExist.PenaltyPriceStepTime = null;
                        checkTimelineExist.HasPenaltyPrice = request.HasPenaltyPrice;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.PenaltyPrice.ToString()))
                        {
                            checkTimelineExist.PenaltyPrice = request.PenaltyPrice;
                        }
                        if (!string.IsNullOrEmpty(request.PenaltyPriceStepTime.ToString()))
                        {
                            checkTimelineExist.PenaltyPriceStepTime = request.PenaltyPriceStepTime;
                        }
                        checkTimelineExist.HasPenaltyPrice = request.HasPenaltyPrice;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.PenaltyPrice.ToString()))
                    {
                        checkTimelineExist.PenaltyPrice = request.PenaltyPrice;
                    }
                    if (!string.IsNullOrEmpty(request.PenaltyPriceStepTime.ToString()))
                    {
                        checkTimelineExist.PenaltyPriceStepTime = request.PenaltyPriceStepTime;
                    }
                }*/
                if(!string.IsNullOrEmpty(request.ExtraFee.ToString()))
                {
                    checkTimelineExist.ExtraFee = request.ExtraFee;
                }
                await _timelineRepository.Update(checkTimelineExist);
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

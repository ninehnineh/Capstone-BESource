using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.UpdateParkingHasPrice
{
    public class UpdateParkingHasPriceCommandHandler : IRequestHandler<UpdateParkingHasPriceCommand, ServiceResponse<string>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly ITimelineRepository _timelineRepository;

        public UpdateParkingHasPriceCommandHandler(IParkingHasPriceRepository parkingHasPriceRepository,
            IParkingPriceRepository parkingPriceRepository,
            IParkingRepository parkingRepository,
            ITimelineRepository timelineRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _parkingRepository = parkingRepository;
            _timelineRepository = timelineRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateParkingHasPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingHasPrice = await _parkingHasPriceRepository.GetById(request.ParkingHasPriceId);
                if (parkingHasPrice == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tồn tại",
                        StatusCode = 200,
                        Success = true
                    };
                }

                if (!string.IsNullOrEmpty(request.ParkingPriceId.ToString()))
                {
                    var parking = await _parkingRepository.GetById(parkingHasPrice.ParkingId!);
                    var checkParkingPriceExist = await _parkingPriceRepository.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true);
                    if (checkParkingPriceExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy gói.",
                            Success = true,
                            StatusCode = 200
                        };
                    }


                    if (checkParkingPriceExist.IsActive == false)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Gói không khả dụng.",
                            StatusCode = 400,
                            Success = false
                        };
                    }

                    if (parking.CarSpot == 0 && checkParkingPriceExist.TrafficId != 1)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Bãi giữ xe không hổ trợ xe hơi nên áp dụng gói không phù hợp.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                    if (parking.MotoSpot == 0 && checkParkingPriceExist.TrafficId != 2)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Bãi giữ xe không hổ trợ xe mô tô nên áp dụng gói không phù hợp.",
                            Success = false,
                            StatusCode = 400
                        };
                    }

                    List<TimeSpan> lstTime = new List<TimeSpan>();
                    var lstTimline = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, null, true);
                    if (lstTimline.FirstOrDefault().StartTime != null && lstTimline.FirstOrDefault().EndTime != null)
                    {
                        foreach (var item in lstTimline)
                        {
                            if (item.EndTime < item.StartTime)
                            {
                                var start = TimeSpan.FromHours(24) - item.StartTime;
                                var end = item.EndTime - TimeSpan.FromHours(0);
                                var result = start + end;
                                lstTime.Add((TimeSpan)result);
                            }
                            else
                            {
                                var result = item.EndTime - item.StartTime;
                                lstTime.Add((TimeSpan)result);
                            }
                        }
                        TimeSpan sumOfTime = new TimeSpan();
                        foreach (var item in lstTime)
                        {
                            sumOfTime += item;
                        }
                        if (parking.IsOvernight == true && sumOfTime.TotalHours != 24)
                        {
                            return new ServiceResponse<string>
                            {
                                Message = "Gói không hợp lệ do bãi có áp dụng giữ 24h nên tổng số giờ của gói phải trong 24h.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                    }
                    else
                    {
                        if (checkParkingPriceExist.IsWholeDay == true && parking.IsOvernight != true)
                        {
                            return new ServiceResponse<string>
                            {
                                Message = "Không thể áp dụng gói cho bãi giữ xe, vì bãi không cho giữ qua đêm mà gói là qua đêm.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                    }

                        
                    parkingHasPrice.ParkingPriceId = request.ParkingPriceId;
                }


                await _parkingHasPriceRepository.Update(parkingHasPrice);
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

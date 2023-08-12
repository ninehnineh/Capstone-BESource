using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                if (checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                

                var checkParkingPriceExist = await _parkingPriceRepository.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true);
                if (checkParkingPriceExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
/*                if (checkParkingExist.IsActive == false)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe không khả dụng.",
                        StatusCode = 400,
                        Success = false
                    };
                }*/
                if (checkParkingPriceExist.IsActive == false)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Gói không khả dụng.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                // Check chỉ áp dụng 1 gói cho 1 loại phương tiện, không thể có 2 gói cùng lúc áp dụng cho 1 phương tiện
                var lstParkingHasPrice = await _parkingHasPriceRepository.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, null, null, true);
                if (lstParkingHasPrice.Count() > 0)
                {
                    foreach (var item in lstParkingHasPrice)
                    {
                        var checkParkingPriceExistVer2 = await _parkingPriceRepository.GetById(item.ParkingPriceId);
                        if(checkParkingPriceExistVer2.TrafficId == checkParkingPriceExist.TrafficId)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Hiện tại đã có gói áp dụng, không thể áp dụng gói này. Nếu muốn sử dụng gói này thì hãy xóa gói đã áp dụng sau đó thêm áp dụng gói này.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                        if(checkParkingPriceExistVer2.IsWholeDay == checkParkingPriceExist.IsWholeDay)
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Hiện tại đã có gói áp dụng, không thể áp dụng gói này. Nếu muốn sử dụng gói này thì hãy xóa gói đã áp dụng sau đó thêm áp dụng gói này.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                    }
                }
                //check traffic of the parking and the timeline has match
                /*if (checkParkingExist.CarSpot != 0 && checkParkingPriceExist.TrafficId != 1)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe chỉ hổ trợ giữ xe hơi. Áp dụng gói không phù hợp.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                if (checkParkingExist.MotoSpot != 0 && checkParkingPriceExist.TrafficId != 2)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi giữ xe chỉ hổ trợ giữ xe máy. Áp dụng gói không phù hợp.",
                        Success = false,
                        StatusCode = 400
                    };
                }*/
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
                if(lstTimline.FirstOrDefault().StartTime != null && lstTimline.FirstOrDefault().EndTime != null)
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

                    if (checkParkingExist.IsOvernight == true && sumOfTime.TotalHours != 24)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Gói không hợp lệ do bãi có áp dụng giữ 24h nên tổng số giờ của gói phải trong 24h.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    if (checkParkingPriceExist.IsWholeDay == true && checkParkingExist.IsOvernight != true)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Không thể áp dụng gói cho bãi giữ xe, vì bãi không cho giữ qua đêm mà gói là qua đêm.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
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

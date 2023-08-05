using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating
{
    public class GetListParkingDesByRatingQueryHandler : IRequestHandler<GetListParkingDesByRatingQuery, ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly ITimelineRepository _timelineRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListParkingDesByRatingQueryHandler(IParkingRepository parkingRepository, IParkingHasPriceRepository parkingHasPriceRepository, IParkingPriceRepository parkingPriceRepository, ITimelineRepository timelineRepository)
        {
            _parkingRepository = parkingRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _timelineRepository = timelineRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>> Handle(GetListParkingDesByRatingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                List<Expression<Func<Domain.Entities.Parking, object>>> includes = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.ParkingHasPrices,
                    x => x.ParkingSpotImages,
                };
                var lstParking = await _parkingRepository.GetAllItemWithPagination(x => x.IsActive == true && x.IsAvailable == true && x.Stars != null && x.Latitude != null && x.Longitude != null && x.ParkingHasPrices.Count() > 0, includes, null, true, request.PageNo, request.PageSize);
                if(lstParking.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>()
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var lstResponse = new List<GetListParkingDesByRatingResponse>();
                var _mapper = config.CreateMapper();
                

                foreach (var item in lstParking)
                {
                    var entity = _mapper.Map<ParkingShowInCusDto>(item);
                    var itemAdd = new GetListParkingDesByRatingResponse();
                    itemAdd.ParkingShowInCusDto = entity;
                    var lstParkingHasPrice = await _parkingHasPriceRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == item.ParkingId);
                    if(lstParkingHasPrice == null)
                    {
                        /*return new ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>
                        {
                            Message = "Bãi chưa cài đặt gói áp dụng.",
                            StatusCode = 400,
                            Success = false
                        };*/
                        itemAdd.PriceCar = null;
                        itemAdd.PriceMoto = null;
                        lstResponse.Add(itemAdd);
                        continue;
                    }
                    foreach (var item2 in lstParkingHasPrice)
                    {
                        var timelineCurrent = await GetTimeLine(item2);
                        var parkingPrice = await _parkingPriceRepository.GetById(item2.ParkingPriceId);
                        if(parkingPrice.TrafficId == 1)
                        {
                            itemAdd.PriceCar = timelineCurrent.Price;
                            itemAdd.PriceMoto = null;
                        }
                        else if(parkingPrice.TrafficId == 2)
                        {
                            itemAdd.PriceMoto = timelineCurrent.Price;
                            itemAdd.PriceCar = null;
                        }
                    }
                    lstResponse.Add(itemAdd);
                }
                return new ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>
                {
                    Data = lstResponse.OrderByDescending(x => x.ParkingShowInCusDto.Stars),
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Count = lstResponse.Count()
                };
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private async Task<TimeLine> GetTimeLine(ParkingHasPrice parkingHasPrice)
        {
            var a = TimeSpan.FromHours(DateTime.UtcNow.AddHours(7).Hour);
            var timelinelst = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == parkingHasPrice.ParkingPriceId);
            foreach (var item in timelinelst)
            {
                if (item.StartTime == null && item.EndTime == null)
                {
                    return item;
                }
                else
                {
                    if (item.StartTime > item.EndTime)
                    {
                        if (item.StartTime > a && item.EndTime >= a)
                        {
                            return item;
                        }
                        else if (item.StartTime <= a && item.EndTime < a)
                        {
                            return item;
                        }
                    }
                    else
                    {
                        if (item.StartTime <= a && item.EndTime >= a)
                        {
                            return item;
                        }
                    }
                }

            }
            return null;
        }
    }
}

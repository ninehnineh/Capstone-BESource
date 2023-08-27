using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models.CalculateDistance;
using Parking.FindingSlotManagement.Domain.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou
{
    public class GetListParkingNearestYouQueryHandler : IRequestHandler<GetListParkingNearestYouQuery, ServiceResponse<IEnumerable<ParkingWithDistance>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListParkingNearestYouQueryHandler(IParkingRepository parkingRepository, ITimelineRepository timelineRepository, IParkingHasPriceRepository parkingHasPriceRepository, IParkingPriceRepository parkingPriceRepository)
        {
            _parkingRepository = parkingRepository;
            _timelineRepository = timelineRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _apiKey = "ulGtxtkmCd8rrbnU2bzRW5FBKYkbKkFL";
            _client = new RestClient("https://api.tomtom.com");
            _httpClient = new HttpClient();
        }
        public async Task<ServiceResponse<IEnumerable<ParkingWithDistance>>> Handle(GetListParkingNearestYouQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.CurrentLatitude.ToString()) || string.IsNullOrEmpty(request.CurrentLongtitude.ToString()) || request.CurrentLatitude == 0 || request.CurrentLongtitude == 0)
                {
                    return new ServiceResponse<IEnumerable<ParkingWithDistance>>
                    {
                        Message = "Dữ liệu không hợp lệ.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var includes = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.ParkingHasPrices,
                    x => x.ParkingSpotImages,
                };

                var lstParking = await _parkingRepository.GetAllItemWithCondition(x => x.IsActive == true && x.IsAvailable == true && x.Latitude != null && x.Longitude != null && x.ParkingHasPrices.Count() > 0 && x.IsFull == false, includes, null, true);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListParkingNearestYouQueryResponse>>(lstParking);
                List<ParkingWithDistance> lst = new();
                foreach (var item in lstDto)
                {
                    var res = await GetDistanceMethod(request.CurrentLatitude, request.CurrentLongtitude, (double)item.Latitude, (double)item.Longitude);

                    var parkingWithDistance = new ParkingWithDistance();
                    var lstParkingHasPrice = await _parkingHasPriceRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == item.ParkingId);
                    if (lstParkingHasPrice == null)
                    {

                        parkingWithDistance = new ParkingWithDistance
                        {
                            GetListParkingNearestYouQueryResponse = item,
                            Distance = res,
                            PriceCar = null
                        };
                        lst.Add(parkingWithDistance);
                        continue;
                    }
                    foreach (var item2 in lstParkingHasPrice)
                    {
                        var timelineCurrent = await GetTimeLine(item2);
                        var parkingPrice = await _parkingPriceRepository.GetById(item2.ParkingPriceId);
                        if (parkingPrice.TrafficId == 1)
                        {
                            parkingWithDistance = new ParkingWithDistance
                            {
                                GetListParkingNearestYouQueryResponse = item,
                                Distance = res,
                                PriceCar = timelineCurrent.Price,
                            };
                        }
                    }
                        
                    lst.Add(parkingWithDistance);
                }
                if (lst.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<ParkingWithDistance>>
                    {
                        Message = "Không tìm thấy bãi đỗ xe nào ở gần bạn.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                return new ServiceResponse<IEnumerable<ParkingWithDistance>>
                {
                    Data = lst.OrderBy(x => x.Distance).Take(5),
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /*private async Task<double> GetDistanceMethod(double lat1, double lon1, double lat2, double lon2)
        {
            var url = $"https://api.tomtom.com/routing/1/calculateRoute/{lat1},{lon1}:{lat2},{lon2}/json";
            var queryString = $"?key={_apiKey}";

            var fullUrl = url + queryString;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Error retrieving response. Status code: {response.StatusCode}");
                }

                var routeResponse = await response.Content.ReadFromJsonAsync<RouteResponse>();

                var distanceInMeters = routeResponse.Routes[0].Summary.LengthInMeters;
                return distanceInMeters / 1000.0; // convert to kilometers
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error processing response.", ex);
            }
        }*/
        /*private double GetDistanceMethod(double lat1, double lon1, double lat2, double lon2)
        {
            var request = new RestRequest($"/routing/1/calculateRoute/{lat1},{lon1}:{lat2},{lon2}/json", Method.Get);
            request.AddParameter("key", _apiKey);

            var response = _client.Execute<RouteResponse>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response.", response.ErrorException);
            }

            var distanceInMeters = response.Data.Routes[0].Summary.LengthInMeters;
            return distanceInMeters / 1000.0; // convert to kilometers
        }*/
        private async Task<double> GetDistanceMethod(double lat1, double lon1, double lat2, double lon2)
        {
            double distance = 0;
            var baseUri = new Uri("https://router.project-osrm.org");
            var coordinates = $"{lon1},{lat1};{lon2},{lat2}";
            var requestUri = new Uri(baseUri, $"/route/v1/driving/{coordinates}");
            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                distance = (double)data.routes[0].distance;
                return (distance / 1000); // convert to kilometers
            }
            return distance;
        }
        private async Task<TimeLine> GetTimeLine(ParkingHasPrice parkingHasPrice)
        {
            var a = TimeSpan.FromHours(DateTime.UtcNow.AddHours(7).Hour);
            var timelinelst = await _timelineRepository.GetAllItemWithCondition(x => x.ParkingPriceId == parkingHasPrice.ParkingPriceId);
            foreach (var item in timelinelst)
            {
                if(item.StartTime == null && item.EndTime == null)
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

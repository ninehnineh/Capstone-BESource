using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models.CalculateDistance;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou
{
    public class GetListParkingNearestYouQueryHandler : IRequestHandler<GetListParkingNearestYouQuery, ServiceResponse<IEnumerable<ParkingWithDistance>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListParkingNearestYouQueryHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
            _apiKey = "ulGtxtkmCd8rrbnU2bzRW5FBKYkbKkFL";
            _client = new RestClient("https://api.tomtom.com");
            _httpClient = new HttpClient();
        }
        public async Task<ServiceResponse<IEnumerable<ParkingWithDistance>>> Handle(GetListParkingNearestYouQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lstParking = await _parkingRepository.GetAllItemWithCondition(x => x.IsActive == true, null, null, true);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListParkingNearestYouQueryResponse>>(lstParking);
                List<ParkingWithDistance> lst = new();
                foreach (var item in lstDto)
                {
                    var res = GetDistanceMethod(request.CurrentLatitude, request.CurrentLongtitude, (double)item.Latitude, (double)item.Longitude);
                    if(res <= 5)
                    {
                        var parkingWithDistance = new ParkingWithDistance
                        {
                            GetListParkingNearestYouQueryResponse = item,
                            Distance = res
                        };
                        lst.Add(parkingWithDistance);
                    }
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
                    Data = lst,
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
        private double GetDistanceMethod(double lat1, double lon1, double lat2, double lon2)
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
        }
        /*private async Task<double> GetDistanceMethod(double lat1, double lon1, double lat2, double lon2)
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
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetDisableParkingHistory
{
    public class GetDisableParkingHistoryQueryHandler : IRequestHandler<GetDisableParkingHistoryQuery, ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>>
    {

        public GetDisableParkingHistoryQueryHandler()
        {
        }
        public async Task<ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>> Handle(GetDisableParkingHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                List<GetDisableParkingHistoryQueryResponse> result = new();

                // var histories = await hangfireRepository.GetHistoryDisableParking(parkingId);
                // var response = new List<GetDisableParkingHistoryQueryResponse>();

                // if (!histories.Any())
                // {
                //     return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                //     {
                //         Message = "Không có dữ liệu",
                //         StatusCode = 200,
                //         Success = true,
                //     };
                // }
                // else
                // {

                //     foreach (var history in histories)
                //     {
                //         var arguments = JArray.Parse(history.Arguments);
                //         disableDate = DateTime.Parse(arguments[1].ToString().Trim('"')).ToString("dd/MM/yyyy");
                //         state = history.StateName;
                //         createdAt = history.CreatedAt.ToString("dd/MM/yyyy");
                //         response.Add(new GetDisableParkingHistoryQueryResponse { State = state, DisableDate = disableDate.ToString(), CreatedAt = createdAt });
                //     }
                // }

                string file1 = "historydisableparking.json";
                if (!File.Exists(file1))
                {
                    return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                    {
                        Message = "Tệp không tồn tại",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                else
                {
                    string jsonFromFile = File.ReadAllText("historydisableparking.json");
                    if (string.IsNullOrWhiteSpace(jsonFromFile))
                    {
                        return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                        {
                            Message = "Tệp rỗng không có dữ liệu",
                            StatusCode = 200,
                            Success = true,
                        };
                    }
                    var scheduledParkingHistoryStatus = ParkingHistoryStatus.Scheduled.ToString();
                    JArray array = JArray.Parse(jsonFromFile);

                    List<JToken> parkings = array.Where(x => x["ParkingId"].Value<int>() == parkingId &&
                                                             x["State"].Value<string>().Equals(scheduledParkingHistoryStatus))
                                                             .ToList();

                    if (parkings.Count() != 0)
                    {
                        foreach (JToken token in parkings)
                        {
                            GetDisableParkingHistoryQueryResponse a = token.ToObject<GetDisableParkingHistoryQueryResponse>();
                            result.Add(a);
                        }
                    }
                    else
                    {
                        return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                        {
                            Message = "Không có dữ liệu",
                            StatusCode = 200,
                            Success = true,
                        };
                    }
                }

                return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                {
                    Data = result,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetDisableParkingHistoryQueryHandler: Message {ex.Message}");
            }

        }
    }
}
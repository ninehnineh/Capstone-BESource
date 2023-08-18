using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json.Linq;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetSuccessedDisableParkingHistory
{
    public class GetSuccessedDisableParkingHistoryQueryHandler : IRequestHandler<GetSuccessedDisableParkingHistoryQuery, ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>>
    {

        public GetSuccessedDisableParkingHistoryQueryHandler()
        {

        }
        public async Task<ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>> Handle(GetSuccessedDisableParkingHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                List<GetSuccessedDisableParkingHistoryQueryResponse> result = new();

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
                    return new ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>
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
                        return new ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>
                        {
                            Message = "Tệp rỗng không có dữ liệu",
                            StatusCode = 200,
                            Success = true,
                        };
                    }
                    var succeededParkingHistoryStatus = ParkingHistoryStatus.Succeeded.ToString();
                    JArray array = JArray.Parse(jsonFromFile);

                    List<JToken> parkings = array.Where(x => x["ParkingId"].Value<int>() == parkingId &&
                                                             x["State"].Value<string>().Equals(succeededParkingHistoryStatus))
                                                             .ToList();

                    if (parkings.Count() != 0)
                    {
                        foreach (JToken token in parkings)
                        {
                            GetSuccessedDisableParkingHistoryQueryResponse a = token.ToObject<GetSuccessedDisableParkingHistoryQueryResponse>();
                            result.Add(a);
                        }
                    }
                    else
                    {
                        return new ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>
                        {
                            Message = "Không có dữ liệu",
                            StatusCode = 200,
                            Success = true,
                        };
                    }
                }

                return new ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>
                {
                    Data = result,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetSuccessedDisableParkingHistoryQueryHandler: Message {ex.Message}");
            }
        }
    }
}
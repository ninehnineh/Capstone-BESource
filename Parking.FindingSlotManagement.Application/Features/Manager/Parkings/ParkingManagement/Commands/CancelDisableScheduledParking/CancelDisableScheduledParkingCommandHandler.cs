using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CancelDisableScheduledParking
{
    public class CancelDisableScheduledParkingCommandHandler : IRequestHandler<CancelDisableScheduledParkingCommand, ServiceResponse<string>>
    {
        private readonly IHangfireRepository hangfireRepository;

        public CancelDisableScheduledParkingCommandHandler(IHangfireRepository hangfireRepository)
        {
            this.hangfireRepository = hangfireRepository;
        }
        public async Task<ServiceResponse<string>> Handle(CancelDisableScheduledParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                var disableDate = request.DisableDate;

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                string file1 = "historydisableparking.json";
                if (!File.Exists(file1))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tệp không tồn tại",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                else
                {
                    string jsonFromFile = File.ReadAllText("historydisableparking.json");
                    JArray array = JArray.Parse(jsonFromFile);
                    // List<JToken> parkings = array.Where(x => x["ParkingId"].Value<int>() == parkingId).ToList();
                    foreach (JToken token in array)
                    {
                        if (token["ParkingId"].Value<int>() == parkingId && token["DisableDate"].Value<string>() == disableDate.Date.ToString("dd/MM/yyyy"))
                        {
                            token.Remove();
                            break;
                        }
                    }

                    string updatedJson = JsonConvert.SerializeObject(array, Formatting.Indented);
                    File.WriteAllText("historydisableparking.json", updatedJson);

                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at CancelDisableScheduledParkingCommandHandler: Message {ex.Message}");
            }
        }
    }
}
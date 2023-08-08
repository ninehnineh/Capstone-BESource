using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json.Linq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetDisableParkingHistory
{
    public class GetDisableParkingHistoryQueryHandler : IRequestHandler<GetDisableParkingHistoryQuery, ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>>
    {
        private readonly IHangfireRepository hangfireRepository;

        public GetDisableParkingHistoryQueryHandler(IHangfireRepository hangfireRepository)
        {
            this.hangfireRepository = hangfireRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>> Handle(GetDisableParkingHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string state;
                string createdAt;
                string disableDate;
                var parkingId = request.ParkingId;
                var histories = await hangfireRepository.GetHistoryDisableParking(parkingId);
                var response = new List<GetDisableParkingHistoryQueryResponse>();

                if (!histories.Any())
                {
                    return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                    {
                        Message = "Không có dữ liệu",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                else
                {

                    foreach (var history in histories)
                    {
                        var arguments = JArray.Parse(history.Arguments);
                        disableDate = DateTime.Parse(arguments[1].ToString().Trim('"')).ToString("dd/MM/yyyy");
                        state = history.StateName;
                        createdAt = history.CreatedAt.ToString("dd/MM/yyyy");
                        response.Add(new GetDisableParkingHistoryQueryResponse { State = state, DisableDate = disableDate, CreatedAt = createdAt });
                    }
                }

                return new ServiceResponse<IEnumerable<GetDisableParkingHistoryQueryResponse>>
                {
                    Data = response,
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
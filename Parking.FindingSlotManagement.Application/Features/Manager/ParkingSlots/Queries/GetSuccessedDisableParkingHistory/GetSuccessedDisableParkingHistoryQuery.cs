using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetSuccessedDisableParkingHistory
{
    public class GetSuccessedDisableParkingHistoryQuery : IRequest<ServiceResponse<IEnumerable<GetSuccessedDisableParkingHistoryQueryResponse>>>
    {
        public int ParkingId { get; set; }
        public string State { get; set; }
    }
}
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice
{
    public class CreateParkingPriceCommand : IRequest<ServiceResponse<int>>
    {
        public string? ParkingPriceName { get; set; }
        public int ManagerId { get; set; }
        public int? TrafficId { get; set; }
        public bool IsWholeDay { get; set; } = false; 
        public int? StartingTime { get; set; }
        public bool? HasPenaltyPrice { get; set; }
        public decimal? PenaltyPrice { get; set; }
        public float? PenaltyPriceStepTime { get; set; }
        public bool? IsExtrafee { get; set; }
        public float? ExtraTimeStep { get; set; }
    }
}

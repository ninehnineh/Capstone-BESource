using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking
{
    public class CreateNewParkingCommand : IRequest<ServiceResponse<int>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? MotoSpot { get; set; } = 0;
        public int? CarSpot { get; set; } = 0;
        public bool? IsPrepayment { get; set; }
        public bool? IsOvernight { get; set; }
        public int? ManagerId { get; set; }
    }
}

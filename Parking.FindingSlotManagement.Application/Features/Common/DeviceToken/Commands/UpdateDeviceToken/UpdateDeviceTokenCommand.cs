using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.DeviceToken.Commands.UpdateDeviceToken
{
    public class UpdateDeviceTokenCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public string? Devicetoken { get; set; }
    }
}

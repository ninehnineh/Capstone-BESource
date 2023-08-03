using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.RatingParking.RatingManagement.Commands.UpdateRatingStarsOfParking
{
    public class UpdateRatingStarsOfParkingCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingId { get; set; }
        public int Stars { get; set; }
    }
}

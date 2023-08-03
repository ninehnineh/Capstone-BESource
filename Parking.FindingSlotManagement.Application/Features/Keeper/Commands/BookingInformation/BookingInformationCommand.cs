using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.BookingInformation
{
    public class BookingInformationCommand : IRequest<ServiceResponse<BookingInformationResponse>>
    {
        public int BookingId { get; set; }

    }
}

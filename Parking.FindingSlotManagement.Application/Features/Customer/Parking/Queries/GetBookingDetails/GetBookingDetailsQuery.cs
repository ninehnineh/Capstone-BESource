﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetBookingDetails
{
    public class GetBookingDetailsQuery : IRequest<ServiceResponse<GetBookingDetailsResponse>>
    {
        public int Bookingid { get; set; }

    }
}

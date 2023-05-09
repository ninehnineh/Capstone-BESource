using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;

public class GetParkingHasPriceDetailWithPaginationQuery :
    IRequest<ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>>
{
    public int ParkingHasPriceId { get; set; }
}

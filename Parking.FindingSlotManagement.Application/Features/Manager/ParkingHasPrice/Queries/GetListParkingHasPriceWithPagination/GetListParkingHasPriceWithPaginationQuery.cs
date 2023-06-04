using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;

public class GetListParkingHasPriceWithPaginationQuery :
    IRequest<ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>>
{   
    public int ParkingId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

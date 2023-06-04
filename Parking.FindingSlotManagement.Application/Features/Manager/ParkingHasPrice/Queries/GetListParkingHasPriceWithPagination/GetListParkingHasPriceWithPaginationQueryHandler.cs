using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;

public class GetListParkingHasPriceWithPaginationQueryHandler :
    IRequestHandler<GetListParkingHasPriceWithPaginationQuery,
        ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>>
{
    private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
    private readonly IMapper _mapper;

    public GetListParkingHasPriceWithPaginationQueryHandler(IParkingHasPriceRepository parkingHasPriceRepository,
        IMapper mapper)
    {   
        _mapper = mapper;
        _parkingHasPriceRepository = parkingHasPriceRepository;
    }

    public async Task<ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>> Handle(GetListParkingHasPriceWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.ParkingHasPrice> listParkingHasPrice = await GetListParkingHasPricesWithPagination(request);

            if (listParkingHasPrice.Count() > 0)
            {
                var response = _mapper.Map<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>(listParkingHasPrice);

                return new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
                {
                    Data = response,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = listParkingHasPrice.Count()
                };
            }

            return new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Không tim thấy",
                Count = 0
            };
        }
        catch (System.Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
    }

    private async Task<IEnumerable<Domain.Entities.ParkingHasPrice>> GetListParkingHasPricesWithPagination(GetListParkingHasPriceWithPaginationQuery request)
    {
        var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
            {
                x => x.Parking!,
                y => y.ParkingPrice!
            };

        var listParkingHasPrice = await _parkingHasPriceRepository
            .GetAllItemWithPagination(
                x => x.ParkingId == request.ParkingId,
                includes,
                x => x.ParkingHasPriceId, true,
                request.PageNo,
                request.PageSize);

        return listParkingHasPrice;
    }
}

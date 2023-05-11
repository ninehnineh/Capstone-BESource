using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;

public class GetParkingHasPriceDetailWithPaginationQueryHandler :
    IRequestHandler<GetParkingHasPriceDetailWithPaginationQuery, ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>>
{
    private readonly IParkingHasPriceRepository parkingHasPriceRepository;
    private readonly IMapper mapper;

    public GetParkingHasPriceDetailWithPaginationQueryHandler(IParkingHasPriceRepository parkingHasPriceRepository,
        IMapper mapper)
    {
        this.mapper = mapper;
        this.parkingHasPriceRepository = parkingHasPriceRepository;
    }

    public async Task<ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>> Handle(GetParkingHasPriceDetailWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
            {
                x => x.Parking!,
                y => y.ParkingPrice!,
            };

            var record = await parkingHasPriceRepository.GetItemWithCondition(x => x.ParkingHasPriceId == request.ParkingHasPriceId, includes);

            if (record != null)
            {
                var response = mapper.Map<GetParkingHasPriceDetailWithPaginationResponse>(record);

                return new ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>
                {
                    Data = response,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành Công",
                    Count = 1
                };
            }

            return new ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>
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
}
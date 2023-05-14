using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice
{
    public class GetAllParkingPriceQueryHandler : IRequestHandler<GetAllParkingPriceQuery, ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IMapper _mapper;

        public GetAllParkingPriceQueryHandler(IParkingPriceRepository parkingPriceRepository,
            IMapper mapper)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>> Handle(GetAllParkingPriceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingPrices = await _parkingPriceRepository
                    .GetAllItemWithPagination(x => x.UserId == request.BusinessId, 
                                                null, x => x.ParkingPriceId, true,
                                                request.PageNo,
                                                request.PageSize);

                if (parkingPrices.Count() < 0)
                {
                    return new ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>
                    {
                        Message = "Danh sách trống",
                        StatusCode = 200,
                        Success = true
                    };
                }

                return new ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>
                {
                    Data = _mapper.Map<IEnumerable<GetAllParkingPriceQueryResponse>>(parkingPrices),
                    Count = parkingPrices.Count(),
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

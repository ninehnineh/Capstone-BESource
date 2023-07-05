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
        private readonly IBusinessProfileRepository _businessProfileRepository;

        public GetAllParkingPriceQueryHandler(IParkingPriceRepository parkingPriceRepository,
            IMapper mapper,
            IBusinessProfileRepository businessProfileRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _mapper = mapper;
            _businessProfileRepository = businessProfileRepository;
        }

        public async Task<ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>> Handle(GetAllParkingPriceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var business = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);

                if (business == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>
                    {
                        Message = "Không tìm thấy",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var parkingPrices = await _parkingPriceRepository
                    .GetAllItemWithPagination(x => x.BusinessId == business.BusinessProfileId,
                                                null, x => x.ParkingPriceId, true,
                                                request.PageNo,
                                                request.PageSize);

                if (parkingPrices.Count() == 0)
                {
                    return new ServiceResponse<IEnumerable<GetAllParkingPriceQueryResponse>>
                    {
                        Message = "Không tìm thấy",
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

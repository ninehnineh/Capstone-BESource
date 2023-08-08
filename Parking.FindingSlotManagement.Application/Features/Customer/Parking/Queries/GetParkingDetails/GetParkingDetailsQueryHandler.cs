using AutoMapper;
using MediatR;
using NuGet.Protocol.Plugins;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails
{
    public class GetParkingDetailsQueryHandler : IRequestHandler<GetParkingDetailsQuery, ServiceResponse<GetParkingDetailsResponse>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        GetParkingDetailsResponse _getParkingDetailsResponse;

        public GetParkingDetailsQueryHandler(IParkingRepository parkingRepository,
            IMapper mapper,
            IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _getParkingDetailsResponse = new GetParkingDetailsResponse();
        }

        public async Task<ServiceResponse<GetParkingDetailsResponse>> Handle(GetParkingDetailsQuery request, CancellationToken cancellationToken)
        {
            var parkingId = request.ParkingId;

            try
            {

                var include = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.ParkingHasPrices,
                    x => x.ParkingSpotImages,
                };

                var parking = await _parkingRepository
                    .GetItemWithCondition(x => x.ParkingId == parkingId, include);

                if (parking == null)
                {
                    return new ServiceResponse<GetParkingDetailsResponse>
                    {
                        Message = "Bãi xe không tồn tại",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var include1 = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
                {
                    x => x.Parking,
                    x => x.ParkingPrice,
                    x => x.ParkingPrice.TimeLines,
                    x => x.ParkingPrice.Traffic,
                };

                var parkingHasPrices = _parkingHasPriceRepository
                    .GetAllItemWithCondition(x => x.ParkingId == parkingId &&
                    x.ParkingPrice.IsActive == true, include1).Result
                    .Where(x => x.ParkingPrice!.TimeLines!.Any(x => x.IsActive == true));

                _getParkingDetailsResponse.Parking = _mapper.Map<ParkingDto>(parking);
                _getParkingDetailsResponse.Parking.ParkingHasPrices = _mapper.Map<IEnumerable<ParkingHasPriceDto>>(parkingHasPrices);

                //var response = _mapper.Map<GetParkingDetailsResponse>(parking);

                return new ServiceResponse<GetParkingDetailsResponse>
                {
                    Data = _getParkingDetailsResponse,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

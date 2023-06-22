using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByParkingPriceId
{
    public class GetListParkingByParkingPriceIdQueryHandler : IRequestHandler<GetListParkingByParkingPriceIdQuery, ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>>
    {
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListParkingByParkingPriceIdQueryHandler(IParkingHasPriceRepository parkingHasPriceRepository, IParkingPriceRepository parkingPriceRepository)
        {
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
        }

        public async Task<ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>> Handle(GetListParkingByParkingPriceIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingPriceExist = await _parkingPriceRepository.GetById(request.ParkingPriceId);
                if (parkingPriceExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>
                    {
                        Message = "Không tìm thấy gói.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>> includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
                {
                    x => x.Parking
                };
                var lstParkingHasPrice = await _parkingHasPriceRepository.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, includes, x => x.ParkingHasPriceId, true);
                if(!lstParkingHasPrice.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>
                    {
                        Message = "Không tìm thấy bãi áp dụng gói.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var lstParkingDto = _mapper.Map<IEnumerable<GetListParkingByParkingPriceIdResponse>>(lstParkingHasPrice);
                return new ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>
                {
                    Data = lstParkingDto,
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

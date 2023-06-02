//using AutoMapper;
//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Mapping;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Queries.GetPackagePriceById
//{
//    public class GetPackagePriceByIdQueryHandler : IRequestHandler<GetPackagePriceByIdQuery, ServiceResponse<GetPackagePriceByIdResponse>>
//    {
//        private readonly IPackagePriceRepository _packagePriceRepository;
//        MapperConfiguration config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new MappingProfile());
//        });

//        public GetPackagePriceByIdQueryHandler(IPackagePriceRepository packagePriceRepository)
//        {
//            _packagePriceRepository = packagePriceRepository;
//        }
//        public async Task<ServiceResponse<GetPackagePriceByIdResponse>> Handle(GetPackagePriceByIdQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                List<Expression<Func<Domain.Entities.TimeLine, object>>> includes = new List<Expression<Func<Domain.Entities.TimeLine, object>>>
//                {
//                    x => x.Traffic
//                };
//                var res = await _packagePriceRepository.GetItemWithCondition(x => x.TimeLineId == request.PackagePriceId, includes, true);
//                if (res == null)
//                {
//                    return new ServiceResponse<GetPackagePriceByIdResponse>
//                    {
//                        Message = "Không tìm thấy gói.",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                var _mapper = config.CreateMapper();
//                var resDto = _mapper.Map<GetPackagePriceByIdResponse>(res);
//                return new ServiceResponse<GetPackagePriceByIdResponse>
//                {
//                    Data = resDto,
//                    Message = "Thành công",
//                    Success = true,
//                    StatusCode = 200
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

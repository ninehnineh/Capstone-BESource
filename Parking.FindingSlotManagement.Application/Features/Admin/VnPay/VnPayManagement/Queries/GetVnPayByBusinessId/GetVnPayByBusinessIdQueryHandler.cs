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

//namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByBusinessId
//{
//    public class GetVnPayByManagerIdQueryHandler : IRequestHandler<GetVnPayByBusinessIdQuery, ServiceResponse<GetVnPayByBusinessIdResponse>>
//    {
//        private readonly IVnPayRepository _vnPayRepository;
//        MapperConfiguration config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new MappingProfile());
//        });

//        public GetVnPayByManagerIdQueryHandler(IVnPayRepository vnPayRepository)
//        {
//            _vnPayRepository = vnPayRepository;
//        }
//        public async Task<ServiceResponse<GetVnPayByBusinessIdResponse>> Handle(GetVnPayByBusinessIdQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                List<Expression<Func<Domain.Entities.VnPay, object>>> includes = new List<Expression<Func<Domain.Entities.VnPay, object>>>
//                {
//                    x => x.Business
//                };
//                var vnpayInfor = await _vnPayRepository.GetItemWithCondition(x => x.BusinessId.Equals(request.BusinessId), includes, true);
//                if (vnpayInfor == null)
//                {
//                    return new ServiceResponse<GetVnPayByBusinessIdResponse>
//                    {
//                        Message = "Không tìm thấy.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                };
//                var _mapper = config.CreateMapper();
//                var vnpayInforEntity = _mapper.Map<GetVnPayByBusinessIdResponse>(vnpayInfor);
//                return new ServiceResponse<GetVnPayByBusinessIdResponse>
//                {
//                    Data = vnpayInforEntity,
//                    Success = true,
//                    StatusCode = 200,
//                    Message = "Thành công"
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

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

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByUserId
{
    public class GetVnPayByManagerIdQueryHandler : IRequestHandler<GetVnPayByUserIdQuery, ServiceResponse<GetVnPayByUserIdResponse>>
    {
        private readonly IVnPayRepository _vnPayRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetVnPayByManagerIdQueryHandler(IVnPayRepository vnPayRepository)
        {
            _vnPayRepository = vnPayRepository;
        }
        public async Task<ServiceResponse<GetVnPayByUserIdResponse>> Handle(GetVnPayByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.VnPay, object>>> includes = new List<Expression<Func<Domain.Entities.VnPay, object>>>
                {
                    x => x.User
                };
                var vnpayInfor = await _vnPayRepository.GetItemWithCondition(x => x.UserId.Equals(request.UserId), includes, true);
                if (vnpayInfor == null)
                {
                    return new ServiceResponse<GetVnPayByUserIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true
                    };
                };
                var _mapper = config.CreateMapper();
                var vnpayInforEntity = _mapper.Map<GetVnPayByUserIdResponse>(vnpayInfor);
                return new ServiceResponse<GetVnPayByUserIdResponse>
                {
                    Data = vnpayInforEntity,
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

using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId
{
    public class GetPaypalByManagerIdQueryHandler : IRequestHandler<GetPaypalByManagerIdQuery, ServiceResponse<GetPaypalByManagerIdResponse>>
    {
        private readonly IPaypalRepository _paypalRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetPaypalByManagerIdQueryHandler(IPaypalRepository paypalRepository)
        {
            _paypalRepository = paypalRepository;
        }
        public async Task<ServiceResponse<GetPaypalByManagerIdResponse>> Handle(GetPaypalByManagerIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<PayPal, object>>> includes = new List<Expression<Func<PayPal, object>>>
                {
                    x => x.Manager
                };
                var paypal = await _paypalRepository.GetItemWithCondition(x => x.ManagerId == request.ManagerId, includes, true);
                if(paypal == null)
                {
                    return new ServiceResponse<GetPaypalByManagerIdResponse>
                    {
                        Message = "Không tìm thấy paypal.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var paypalDto = _mapper.Map<GetPaypalByManagerIdResponse>(paypal);
                return new ServiceResponse<GetPaypalByManagerIdResponse>
                {
                    Data = paypalDto,
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

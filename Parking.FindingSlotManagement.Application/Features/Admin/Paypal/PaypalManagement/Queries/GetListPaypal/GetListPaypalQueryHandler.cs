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

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal
{
    public class GetListPaypalQueryHandler : IRequestHandler<GetListPaypalQuery, ServiceResponse<IEnumerable<GetListPaypalResponse>>>
    {
        private readonly IPaypalRepository _paypalRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListPaypalQueryHandler(IPaypalRepository paypalRepository)
        {
            _paypalRepository = paypalRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListPaypalResponse>>> Handle(GetListPaypalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                List<Expression<Func<PayPal, object>>> includes = new List<Expression<Func<PayPal, object>>>
                {
                    x => x.Manager
                };
                var lst = await _paypalRepository.GetAllItemWithPagination(null, includes, x => x.PayPalId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListPaypalResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListPaypalResponse>>
                    {
                        Message = "Không tìm thấy",
                        StatusCode = 200,
                        Success = true
                    };
                }
                return new ServiceResponse<IEnumerable<GetListPaypalResponse>>
                {
                    Success = true,
                    Data = lstDto,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = lst.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills
{
    public class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsQuery, ServiceResponse<IEnumerable<GetAllBillsResponse>>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetAllBillsQueryHandler(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllBillsResponse>>> Handle(GetAllBillsQuery request, CancellationToken cancellationToken)
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
                List<Expression<Func<Domain.Entities.Bill, object>>> includes = new()
                {
                    x => x.businessProfile
                };
                var lst = await _billRepository.GetAllItemWithPagination(null, includes, x => x.BillId, true, request.PageNo, request.PageSize);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetAllBillsResponse>>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 404,
                        Success = false
                    };
                }
                var lstDto = _mapper.Map<IEnumerable<GetAllBillsResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetAllBillsResponse>>
                {
                    Data = lstDto,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = lstDto.Count()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

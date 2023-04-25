using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries
{
    public class GetRequestAccountListQueryHandler : IRequestHandler<GetRequestAccountListQuery, ServiceResponse<IEnumerable<RequestResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetRequestAccountListQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<IEnumerable<RequestResponse>>> Handle(GetRequestAccountListQuery request, CancellationToken cancellationToken)
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
                var lst = await _accountRepository.GetAllItemWithPagination(x => x.IsCensorship == false && x.IsActive == true, null, x => x.UserId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<RequestResponse>>(lst);
                if (lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<RequestResponse>>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Không tìm thấy",
                        Count = lstDto.Count()
                    };
                }
                return new ServiceResponse<IEnumerable<RequestResponse>>
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

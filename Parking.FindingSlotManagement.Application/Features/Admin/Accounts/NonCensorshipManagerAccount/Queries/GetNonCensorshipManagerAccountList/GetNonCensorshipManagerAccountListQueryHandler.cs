using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.NonCensorshipManagerAccount.Queries.GetNonCensorshipManagerAccountList
{
    public class GetNonCensorshipManagerAccountListQueryHandler : IRequestHandler<GetNonCensorshipManagerAccountListQuery, ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetNonCensorshipManagerAccountListQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>> Handle(GetNonCensorshipManagerAccountListQuery request, CancellationToken cancellationToken)
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
                var lst = await _accountRepository.GetAllItemWithPagination(x => x.IsCensorship == false && x.IsActive == false, null, x => x.UserId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<NonCensorshipManagerAccountResponse>>(lst);
                var countFromList = lstDto.Count();
                if (countFromList <= 0)
                {
                    return new ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Không tìm thấy",
                        Count = countFromList
                    };
                }
                return new ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>
                {
                    Data = lstDto,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = countFromList
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

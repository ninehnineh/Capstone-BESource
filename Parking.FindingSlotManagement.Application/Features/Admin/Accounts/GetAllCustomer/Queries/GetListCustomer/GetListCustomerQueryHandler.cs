using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetListCustomer
{
    public class GetListCustomerQueryHandler : IRequestHandler<GetListCustomerQuery, ServiceResponse<IEnumerable<GetListCustomerResponse>>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListCustomerQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListCustomerResponse>>> Handle(GetListCustomerQuery request, CancellationToken cancellationToken)
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
                var lst = await _userRepository.GetAllItemWithPagination(x => x.RoleId == 3, null, x => x.UserId, true, request.PageNo, request.PageSize);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListCustomerResponse>>
                    {
                        Message = "Danh sách trống.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListCustomerResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetListCustomerResponse>>
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

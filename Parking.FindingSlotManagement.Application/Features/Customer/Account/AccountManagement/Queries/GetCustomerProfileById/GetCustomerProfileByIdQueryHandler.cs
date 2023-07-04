using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetCustomerProfileById
{
    public class GetCustomerProfileByIdQueryHandler : IRequestHandler<GetCustomerProfileByIdQuery, ServiceResponse<GetCustomerProfileByIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetCustomerProfileByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<GetCustomerProfileByIdResponse>> Handle(GetCustomerProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetById(request.UserId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<GetCustomerProfileByIdResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var entityDto = _mapper.Map<GetCustomerProfileByIdResponse>(checkUserExist);
                return new ServiceResponse<GetCustomerProfileByIdResponse>
                {
                    Data = entityDto,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Commands.UpdateCustomerProfileById
{
    public class UpdateCustomerProfileByIdCommandHandler : IRequestHandler<UpdateCustomerProfileByIdCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateCustomerProfileByIdCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateCustomerProfileByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetById(request.UserId);
                if (checkUserExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))
                {
                    checkUserExist.Name = request.Name;
                }
                if(!string.IsNullOrEmpty(request.Avatar))
                {
                    checkUserExist.Avatar = request.Avatar;
                }
                if (!string.IsNullOrEmpty(request.DateOfBirth.ToString()))
                {
                    checkUserExist.DateOfBirth = request.DateOfBirth;
                }
                if (!string.IsNullOrEmpty(request.Gender))
                {
                    checkUserExist.Gender = request.Gender;
                }
                if (!string.IsNullOrEmpty(request.Address))
                {
                    checkUserExist.Address = request.Address;
                }
                await _userRepository.Update(checkUserExist);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
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

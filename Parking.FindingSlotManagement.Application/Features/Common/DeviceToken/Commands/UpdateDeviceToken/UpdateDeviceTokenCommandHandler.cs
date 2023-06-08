using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.DeviceToken.Commands.UpdateDeviceToken
{
    public class UpdateDeviceTokenCommandHandler : IRequestHandler<UpdateDeviceTokenCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateDeviceTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateDeviceTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetById(request.UserId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkUserExist.Devicetoken == null)
                {
                    checkUserExist.Devicetoken = request.Devicetoken;
                    await _userRepository.Save();
                    return new ServiceResponse<string>
                    {
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 204
                    };
                }
                if(checkUserExist.Devicetoken.Equals(request.Devicetoken))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 204
                    };
                }
                checkUserExist.Devicetoken = request.Devicetoken;
                await _userRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.AcceptParkingRequest
{
    public class AcceptParkingRequestCommandHandler : IRequestHandler<AcceptParkingRequestCommand, ServiceResponse<string>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IEmailService _emailService;

        public AcceptParkingRequestCommandHandler(IApproveParkingRepository approveParkingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository, IParkingRepository parkingRepository, IEmailService emailService)
        {
            _approveParkingRepository = approveParkingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
            _parkingRepository = parkingRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<string>> Handle(AcceptParkingRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var approveParking = await _approveParkingRepository.GetItemWithCondition(x => x.ApproveParkingId == request.ApproveParkingId, null, false);
                if(approveParking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy yêu cầu xác thực của bãi.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if(!approveParking.Status.Equals(Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString()))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var parkingExist = await _parkingRepository.GetById(approveParking.ParkingId);
                parkingExist.IsActive = true;
                parkingExist.IsAvailable = true;
                await _parkingRepository.Save();
                approveParking.NoteForAdmin = request.NoteForAdmin;
                approveParking.Status = Domain.Enum.ApproveParkingStatus.Đã_duyệt.ToString();
                await _approveParkingRepository.Save();
                var getBusinessExist = await _businessProfileRepository.GetById(parkingExist.BusinessId);
                if(getBusinessExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy doanh nghiệp.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var userExist = await _userRepository.GetById(getBusinessExist.UserId);
                if(userExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                EmailModel emailModel = new EmailModel();
                emailModel.To = userExist.Email;
                emailModel.Subject = "Thông báo: Yêu cầu duyệt bãi đã được chấp nhận";

                string body = $"Dear {userExist.Name},\n\n";
                body += "Chúng tôi xin thông báo rằng hệ thống của chúng tôi đã chấp nhận yêu cầu duyệt bãi .\n"+parkingExist.Name;
                body += "Chân thành cảm ơn sự tin tưởng và ủng hộ của bạn đối với hệ thống của chúng tôi.\n\n";
                body += "Trân trọng,\n";
                body += "ParkZ\n";
                body += "Địa chỉ công ty: Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000\n";
                body += "Số điện thoại công ty: 0793808821\n";
                body += "Địa chỉ email công ty: parkz.thichthicodeteam@gmail.com\r\n";
                emailModel.Body = body;
                _emailService.SendMail(emailModel);
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

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.SendRequestApproveParking
{
    public class SendRequestApproveParkingCommandHandler : IRequestHandler<SendRequestApproveParkingCommand, ServiceResponse<string>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;

        public SendRequestApproveParkingCommandHandler(IApproveParkingRepository approveParkingRepository, IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _approveParkingRepository = approveParkingRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
        }
        public async Task<ServiceResponse<string>> Handle(SendRequestApproveParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkRequestExist = await _approveParkingRepository.GetById(request.ApproveParkingId);
                if (checkRequestExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy yêu cầu.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkRequestExist.Status.Equals(ApproveParkingStatus.Đã_duyệt.ToString()))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không thể gửi yêu cầu duyệt.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var parkingHasPriceExist = await _parkingHasPriceRepository.GetItemWithCondition(x => x.ParkingId == checkRequestExist.ParkingId);
                if(parkingHasPriceExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi chưa áp dụng gói cước nên không thể gửi duyệt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                checkRequestExist.Status = ApproveParkingStatus.Chờ_duyệt.ToString();
                await _approveParkingRepository.Save();
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

using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.UpdateApproveParking
{
    public class UpdateApproveParkingCommandHandler : IRequestHandler<UpdateApproveParkingCommand, ServiceResponse<int>>
    {

        private readonly IApproveParkingRepository _approveParkingRepository;

        public UpdateApproveParkingCommandHandler(IApproveParkingRepository approveParkingRepository)
        {
            _approveParkingRepository = approveParkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(UpdateApproveParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var approveParking = await _approveParkingRepository.GetItemWithCondition(x => x.ApproveParkingId == request.ApproveParkingId, null, false);
                if (approveParking == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy yêu cầu xác thực của bãi.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (!approveParking.Status.Equals(Domain.Enum.ApproveParkingStatus.Tạo_mới.ToString()))
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                if(!string.IsNullOrEmpty(request.Note))
                {
                    approveParking.Note = request.Note;
                }
                await _approveParkingRepository.Update(approveParking);
                return new ServiceResponse<int>
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

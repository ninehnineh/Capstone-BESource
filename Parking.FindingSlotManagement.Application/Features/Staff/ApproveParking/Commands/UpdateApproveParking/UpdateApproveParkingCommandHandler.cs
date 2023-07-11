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
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IUserRepository _userRepository;

        public UpdateApproveParkingCommandHandler(IFieldWorkParkingImgRepository fieldWorkParkingImgRepository, IApproveParkingRepository approveParkingRepository, IParkingRepository parkingRepository, IUserRepository userRepository)
        {
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
            _approveParkingRepository = approveParkingRepository;
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<int>> Handle(UpdateApproveParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var approveParking = await _approveParkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId, null, false);
                if (approveParking == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy yêu cầu xác thực của bãi.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (!approveParking.Status.Equals(Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString()))
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var staffExist = await _userRepository.GetById(request.StaffId);
                if(staffExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(staffExist.RoleId != 4)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bạn không có quyền truy cập.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                approveParking.StaffId = request.StaffId;
                approveParking.Note = request.Note;
                await _approveParkingRepository.Update(approveParking);
                List<FieldWorkParkingImg> lstFWPI = new();
                foreach (var item in request.Images)
                {
                    FieldWorkParkingImg fwpi = new FieldWorkParkingImg
                    {
                        Url = item,
                        ApproveParkingId = approveParking.ApproveParkingId
                    };
                    lstFWPI.Add(fwpi);
                }
                await _fieldWorkParkingImgRepository.AddRangeFieldWorkParkingImg(lstFWPI);
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

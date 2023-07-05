using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking
{
    public class CreateNewApproveParkingCommandHandler : IRequestHandler<CreateNewApproveParkingCommand, ServiceResponse<int>>
    {
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IUserRepository _userRepository;

        public CreateNewApproveParkingCommandHandler(IFieldWorkParkingImgRepository fieldWorkParkingImgRepository, IApproveParkingRepository approveParkingRepository, IParkingRepository parkingRepository, IUserRepository userRepository)
        {
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
            _approveParkingRepository = approveParkingRepository;
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewApproveParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(parkingExist.IsActive == true)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi đã được duyệt.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var ap = new Domain.Entities.ApproveParking
                {
                    StaffId = request.StaffId,
                    ParkingId = request.ParkingId,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    Note = request.Note,
                    Status = Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString()
                };
                await _approveParkingRepository.Insert(ap);
                List<FieldWorkParkingImg> lstFWPI = new();
                foreach (var item in request.Images)
                {
                    FieldWorkParkingImg fwpi = new FieldWorkParkingImg
                    {
                        Url = item,
                        ApproveParkingId = ap.ApproveParkingId
                    };
                    lstFWPI.Add(fwpi);
                }
                await _fieldWorkParkingImgRepository.AddRangeFieldWorkParkingImg(lstFWPI);
                return new ServiceResponse<int>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201,
                    Data = ap.ApproveParkingId
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

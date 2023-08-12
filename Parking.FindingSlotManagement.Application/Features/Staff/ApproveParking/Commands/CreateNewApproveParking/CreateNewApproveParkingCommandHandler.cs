using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking
{
    public class CreateNewApproveParkingCommandHandler : IRequestHandler<CreateNewApproveParkingCommand, ServiceResponse<int>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;

        public CreateNewApproveParkingCommandHandler(IApproveParkingRepository approveParkingRepository, IUserRepository userRepository, IParkingRepository parkingRepository, IMapper mapper, IFieldWorkParkingImgRepository fieldWorkParkingImgRepository)
        {
            _approveParkingRepository = approveParkingRepository;
            _userRepository = userRepository;
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewApproveParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkStaffExist = await _userRepository.GetById(request.StaffId);
                if(checkStaffExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkStaffExist.RoleId != 4)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tài khoản của bạn không phải là thực địa của hệ thông. Không được phép truy cập.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkParkingExist.IsActive == true)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi đã được xác thực.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var lstApproveParkingManagement = await _approveParkingRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == request.ParkingId);
                if (lstApproveParkingManagement == null)
                {
                    var approveParkingEntity = _mapper.Map<Domain.Entities.ApproveParking>(request);
                    approveParkingEntity.Status = ApproveParkingStatus.Tạo_mới.ToString();
                    approveParkingEntity.CreatedDate = DateTime.UtcNow.AddHours(7);
                    approveParkingEntity.NoteForAdmin = null;
                    await _approveParkingRepository.Insert(approveParkingEntity);
                    if (!request.Images.Any())
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Hãy nhập ảnh thực địa về bãi xe.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                    List<Domain.Entities.FieldWorkParkingImg> lstFWPI = new();
                    foreach (var item in request.Images)
                    {
                        Domain.Entities.FieldWorkParkingImg fwpi = new Domain.Entities.FieldWorkParkingImg
                        {
                            Url = item,
                            ApproveParkingId = approveParkingEntity.ApproveParkingId
                        };
                        lstFWPI.Add(fwpi);
                    }
                    await _fieldWorkParkingImgRepository.AddRangeFieldWorkParkingImg(lstFWPI);
                    return new ServiceResponse<int>
                    {
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 201,
                        Data = approveParkingEntity.ApproveParkingId
                    };
                }
                if (lstApproveParkingManagement.Any())
                {
                    foreach (var item in lstApproveParkingManagement)
                    {
                        if(item.Status.Equals(ApproveParkingStatus.Tạo_mới.ToString()))
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Đang có yêu cầu tạo mới, không thể tạo thêm yêu cầu.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                    }
                    if(lstApproveParkingManagement.LastOrDefault().Status.Equals(ApproveParkingStatus.Từ_chối.ToString()))
                    {
                        var approveParkingEntity = _mapper.Map<Domain.Entities.ApproveParking>(request);
                        approveParkingEntity.Status = ApproveParkingStatus.Tạo_mới.ToString();
                        approveParkingEntity.CreatedDate = DateTime.UtcNow.AddHours(7);
                        approveParkingEntity.NoteForAdmin = null;
                        await _approveParkingRepository.Insert(approveParkingEntity);
                        if (!request.Images.Any())
                        {
                            return new ServiceResponse<int>
                            {
                                Message = "Hãy nhập ảnh thực địa về bãi xe.",
                                Success = false,
                                StatusCode = 400
                            };
                        }
                        List<Domain.Entities.FieldWorkParkingImg> lstFWPI = new();
                        foreach (var item in request.Images)
                        {
                            Domain.Entities.FieldWorkParkingImg fwpi = new Domain.Entities.FieldWorkParkingImg
                            {
                                Url = item,
                                ApproveParkingId = approveParkingEntity.ApproveParkingId
                            };
                            lstFWPI.Add(fwpi);
                        }
                        await _fieldWorkParkingImgRepository.AddRangeFieldWorkParkingImg(lstFWPI);
                        return new ServiceResponse<int>
                        {
                            Message = "Thành công",
                            Success = true,
                            StatusCode = 201,
                            Data = approveParkingEntity.ApproveParkingId
                        };
                    }
                }
                
                
                return new ServiceResponse<int>
                {
                    Message = "Đã xảy ra lỗi",
                    Success = false,
                    StatusCode = 400,
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

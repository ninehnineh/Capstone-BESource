using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking
{
    public class CreateNewParkingCommandHandler : IRequestHandler<CreateNewParkingCommand, ServiceResponse<int>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IFeeRepository _feeRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewParkingCommandHandler(IParkingRepository parkingRepository,
            IBusinessProfileRepository businessProfileRepository,
            IUserRepository userRepository,
            IApproveParkingRepository approveParkingRepository, IFeeRepository feeRepository)
        {
            _parkingRepository = parkingRepository;
            _businessProfileRepository = businessProfileRepository;
            _userRepository = userRepository;
            _approveParkingRepository = approveParkingRepository;
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _parkingRepository.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true);
                if(checkExist != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tên bãi xe đã tồn tại. Vui lòng nhập tên bãi xe khác.",
                        StatusCode = 400,
                        Success = false,
                        Count = 0
                    };
                }

                var checkBusinessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);
                if(checkBusinessExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var countParking = await _parkingRepository.GetAllItemWithConditionByNoInclude(x => x.BusinessId == checkBusinessExist.BusinessProfileId);
                var feeExist = await _feeRepository.GetById(checkBusinessExist.FeeId);
                if(feeExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Doanh nghiệp chưa áp dụng gói",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if(feeExist.BusinessType.Equals("Tư nhân"))
                {
                    if(countParking == null)
                    {
                        var _mapper = config.CreateMapper();
                        var parkingEntity = _mapper.Map<Domain.Entities.Parking>(request);
                        parkingEntity.BusinessId = checkBusinessExist.BusinessProfileId;
                        parkingEntity.IsActive = false;
                        parkingEntity.IsFull = false;
                        parkingEntity.IsAvailable = false;
                        await _parkingRepository.Insert(parkingEntity);
                        parkingEntity.Code = "BX" + parkingEntity.ParkingId;
                        parkingEntity.Stars = (float)0.0;
                        await _parkingRepository.Save();
                        return new ServiceResponse<int>
                        {
                            Data = parkingEntity.ParkingId,
                            Message = "Thành công",
                            Success = true,
                            StatusCode = 201,
                            Count = 0
                        };
                    }
                    if(countParking.Count() >= 1)
                    {
                        return new ServiceResponse<int>
                        {
                            Message = "Bạn không thể tạo thêm bãi xe. Do bạn đã sử dụng gói tư nhân chỉ được tạo tối đa 1 bãi xe.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                    
                }
                else if(feeExist.BusinessType.Equals("Doanh nghiệp"))
                {
                    var _mapper = config.CreateMapper();
                    var parkingEntity = _mapper.Map<Domain.Entities.Parking>(request);
                    parkingEntity.BusinessId = checkBusinessExist.BusinessProfileId;
                    parkingEntity.IsActive = false;
                    parkingEntity.IsFull = false;
                    parkingEntity.IsAvailable = false;
                    await _parkingRepository.Insert(parkingEntity);
                    parkingEntity.Code = "BX" + parkingEntity.ParkingId;
                    parkingEntity.Stars = (float)0.0;
                    await _parkingRepository.Save();
                    return new ServiceResponse<int>
                    {
                        Data = parkingEntity.ParkingId,
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 201,
                        Count = 0
                    };
                }


                return new ServiceResponse<int>
                {
                    Message = "Có lỗi xảy ra",
                    Success = false,
                    StatusCode = 400,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

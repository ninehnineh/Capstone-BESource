using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
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
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewParkingCommandHandler(IParkingRepository parkingRepository,
            IBusinessProfileRepository businessProfileRepository,
            IUserRepository userRepository,
            IApproveParkingRepository approveParkingRepository)
        {
            _parkingRepository = parkingRepository;
            _businessProfileRepository = businessProfileRepository;
            _userRepository = userRepository;
            _approveParkingRepository = approveParkingRepository;
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


                var _mapper = config.CreateMapper();
                var parkingEntity = _mapper.Map<Domain.Entities.Parking>(request);
                parkingEntity.BusinessId = checkBusinessExist.BusinessProfileId;
                parkingEntity.IsActive = false;
                parkingEntity.IsFull = false;
                await _parkingRepository.Insert(parkingEntity);
                parkingEntity.Code = "BX" + parkingEntity.ParkingId;
                parkingEntity.Stars = (float)0.0;
                await _parkingRepository.Save();
                /*var managerExist = await _userRepository.GetById(checkBusinessExist.UserId);
                managerExist.ParkingId = parkingEntity.ParkingId;

                await _userRepository.Save();*/
                return new ServiceResponse<int>
                {
                    Data = parkingEntity.ParkingId,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201,
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

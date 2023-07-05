using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice
{
    public class CreateParkingPriceCommandHandler : IRequestHandler<CreateParkingPriceCommand, ServiceResponse<int>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateParkingPriceCommandHandler(IParkingPriceRepository parkingPriceRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
        }

        public async Task<ServiceResponse<int>> Handle(CreateParkingPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.HasPenaltyPrice == false)
                {
                    request.PenaltyPrice = null;
                    request.PenaltyPriceStepTime = null;
                }
                if(request.IsExtrafee == false)
                {
                    request.ExtraTimeStep = null;
                }
                var managerExist = await _userRepository.GetById(request.ManagerId);
                if(managerExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = " Không tìm thấy tài khoản doanh nghiệp.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(managerExist.RoleId != 1)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tài khoản không phải là doanh nghiệp.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == managerExist.UserId);
                if(businessExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var entityConvert = new CreateParkingPriceCommandDTO
                {
                    ParkingPriceName = request.ParkingPriceName,
                    BusinessId = businessExist.BusinessProfileId,
                    TrafficId = request.TrafficId,
                    IsWholeDay = request.IsWholeDay,
                    StartingTime = request.StartingTime,
                    HasPenaltyPrice = request.HasPenaltyPrice,
                    PenaltyPrice = request.PenaltyPrice,
                    PenaltyPriceStepTime = request.PenaltyPriceStepTime,
                    IsExtrafee = request.IsExtrafee,
                    ExtraTimeStep = request.ExtraTimeStep
                };
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<Domain.Entities.ParkingPrice>(entityConvert);

                entity.IsActive = true;

                await _parkingPriceRepository.Insert(entity);

                return new ServiceResponse<int>
                {
                    Data = entity.ParkingPriceId,
                    Message = "Thành công",
                    StatusCode = 201,
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

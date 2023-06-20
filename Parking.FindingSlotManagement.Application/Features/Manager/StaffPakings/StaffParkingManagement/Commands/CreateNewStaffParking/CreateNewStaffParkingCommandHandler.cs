//using AutoMapper;
//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Mapping;
//using Parking.FindingSlotManagement.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.CreateNewStaffParking
//{
//    public class CreateNewStaffParkingCommandHandler : IRequestHandler<CreateNewStaffParkingCommand, ServiceResponse<int>>
//    {
//        private readonly IStaffParkingRepository _staffParkingRepository;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IParkingRepository _parkingRepository;
//        MapperConfiguration config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new MappingProfile());
//        });

//        public CreateNewStaffParkingCommandHandler(IStaffParkingRepository staffParkingRepository, IAccountRepository accountRepository, IParkingRepository parkingRepository)
//        {
//            _staffParkingRepository = staffParkingRepository;
//            _accountRepository = accountRepository;
//            _parkingRepository = parkingRepository;
//        }
//        public async Task<ServiceResponse<int>> Handle(CreateNewStaffParkingCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var checkUserExist = await _accountRepository.GetById(request.UserId);
//                if (checkUserExist == null)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Không tìm thấy tài khoản.",
//                        Success = true,
//                        StatusCode = 200,
//                        Count = 0
//                    };
//                }
//                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
//                if (checkParkingExist == null)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Không tìm thấy bãi giữ xe.",
//                        Success = true,
//                        StatusCode = 200,
//                        Count = 0
//                    };
//                }
//                var _mapper = config.CreateMapper();
//                var staffParkingEntity = _mapper.Map<StaffParking>(request);
//                await _staffParkingRepository.Insert(staffParkingEntity);
//                return new ServiceResponse<int>
//                {
//                    Data = staffParkingEntity.StaffParkingId,
//                    Message = "Thành công",
//                    StatusCode = 201,
//                    Count = 0,
//                    Success = true
//                };

//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

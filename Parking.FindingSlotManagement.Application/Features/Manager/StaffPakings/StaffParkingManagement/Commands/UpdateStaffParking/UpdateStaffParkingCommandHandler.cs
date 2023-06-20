//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.UpdateStaffParking
//{
//    public class UpdateStaffParkingCommandHandler : IRequestHandler<UpdateStaffParkingCommand, ServiceResponse<string>>
//    {
//        private readonly IStaffParkingRepository _staffParkingRepository;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IParkingRepository _parkingRepository;

//        public UpdateStaffParkingCommandHandler(IStaffParkingRepository staffParkingRepository, IAccountRepository accountRepository, IParkingRepository parkingRepository)
//        {
//            _staffParkingRepository = staffParkingRepository;
//            _accountRepository = accountRepository;
//            _parkingRepository = parkingRepository;
//        }
//        public async Task<ServiceResponse<string>> Handle(UpdateStaffParkingCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var checkExist = await _staffParkingRepository.GetById(request.StaffParkingId);
//                if(checkExist == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy.",
//                        Success = true,
//                        StatusCode = 200,
//                        Count = 0
//                    };
//                }
//                if(!string.IsNullOrEmpty(request.UserId.ToString()))
//                {
//                    var checkUserExist = await _accountRepository.GetById(request.UserId);
//                    if (checkUserExist == null)
//                    {
//                        return new ServiceResponse<string>
//                        {
//                            Message = "Không tìm thấy tài khoản.",
//                            Success = true,
//                            StatusCode = 200,
//                            Count = 0
//                        };
//                    }
//                    checkExist.UserId = request.UserId;
//                }
//                if(!string.IsNullOrEmpty(request.ParkingId.ToString()))
//                {
//                    var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
//                    if (checkParkingExist == null)
//                    {
//                        return new ServiceResponse<string>
//                        {
//                            Message = "Không tìm thấy bãi giữ xe.",
//                            Success = true,
//                            StatusCode = 200,
//                            Count = 0
//                        };
//                    }
//                    checkExist.ParkingId = request.ParkingId;
//                }
//                await _staffParkingRepository.Update(checkExist);
//                return new ServiceResponse<string>
//                {
//                    Message = "Thành công",
//                    Success = true,
//                    StatusCode = 204,
//                    Count = 0
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

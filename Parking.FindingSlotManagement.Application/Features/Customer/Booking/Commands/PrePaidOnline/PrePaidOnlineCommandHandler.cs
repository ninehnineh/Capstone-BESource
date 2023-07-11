//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Models.Booking;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Parking.FindingSlotManagement.Domain.Enum;
//using SharpCompress.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PrePaidOnline
//{
//    public class PrePaidOnlineCommandHandler : IRequestHandler<PrePaidOnlineCommand, ServiceResponse<string>>
//    {
//        private readonly IVnPayService _vnPayService;
//        private readonly IParkingRepository _parkingRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly IBusinessProfileRepository _businessProfileRepository;

//        public PrePaidOnlineCommandHandler(IVnPayService vnPayService,
//            IParkingRepository parkingRepository,
//            IUserRepository userRepository,
//            IBusinessProfileRepository businessProfileRepository)
//        {
//            _vnPayService = vnPayService;
//            _parkingRepository = parkingRepository;
//            _userRepository = userRepository;
//            _businessProfileRepository = businessProfileRepository;
//        }
//        public async Task<ServiceResponse<string>> Handle(PrePaidOnlineCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var parking = await _parkingRepository.GetById(request.ParkingId);
//                if (parking == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy bãi giữ xe.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                }
//                var business = await _businessProfileRepository.GetById(parking.BusinessId);
//                if (business == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                }
//                var includesUser = new List<Expression<Func<Domain.Entities.User, object>>>
//                {
//                    x => x.Wallet
//                };
//                var manager = await _userRepository
//                    .GetItemWithCondition(x => x.UserId == business.UserId && x.RoleId == 1, includesUser);
//                if (manager == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Tài khoản manager không tồn tại.",
//                        StatusCode = 200,
//                        Success = true,
//                    };
//                }
//                var userBooking = await _userRepository
//                    .GetItemWithCondition(x => x.UserId == request.UserId, includesUser, false);
//                //tiền trong ví đủ để thanh toán
//                if (userBooking.Wallet.Balance >= request.TotalPrice && userBooking.Wallet.Debt == 0)
//                {
//                    userBooking.Wallet.Balance -= (decimal)request.TotalPrice;
//                    manager.Wallet.Balance += (decimal)request.TotalPrice;
//                    await _userRepository.Save();
//                    await _userRepository.Update(manager);
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Thành công",
//                        StatusCode = 201,
//                        Success = true
//                    };
//                }
//                else if (userBooking.Wallet.Balance < request.TotalPrice)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Số dư trong ví không đủ để thanh toán.",
//                        StatusCode = 400,
//                        Success = false
//                    };
//                }
//                return new ServiceResponse<string>
//                {
//                    Message = "Thành công",
//                    StatusCode = 201,
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

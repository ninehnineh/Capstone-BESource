//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Models.Booking;
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

//        public PrePaidOnlineCommandHandler(IVnPayService vnPayService, IParkingRepository parkingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
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
//                var manager = await _userRepository.GetItemWithCondition(x => x.ParkingId == parking.ParkingId && x.RoleId == 1);
//                if (manager == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Tài khoản manager không tồn tại.",
//                        StatusCode = 200,
//                        Success = true,
//                    };
//                }
//                List<Expression<Func<Domain.Entities.BusinessProfile, object>>> includesUser = new List<Expression<Func<Domain.Entities.BusinessProfile, object>>>
//                {
//                    x => x.VnPays
//                };
//                var businessAcc = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == manager.UserId, includesUser, true);
//                if (businessAcc == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                }
//                BookingTransaction bt = new BookingTransaction
//                {
//                    ParkingSlotName = request.ParkingSlotName,
//                    TotalPrice = (decimal)request.TotalPrice
//                };
//                var url = _vnPayService.CreatePaymentUrl(bt, businessAcc.VnPays.FirstOrDefault().TmnCode, businessAcc.VnPays.FirstOrDefault().HashSecret, request.context);
//                return new ServiceResponse<string>
//                {
//                    Data = url,
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

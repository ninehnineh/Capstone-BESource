//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Models.Booking;
//using Parking.FindingSlotManagement.Domain.Enum;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PostPaidOnline
//{
//    public class PostPaidOnlineCommandHandler : IRequestHandler<PostPaidOnlineCommand, ServiceResponse<string>>
//    {
//        private readonly IBookingRepository _bookingRepository;
//        private readonly IVnPayService _vnPayService;
//        private readonly IParkingRepository _parkingRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly IBusinessProfileRepository _businessProfileRepository;

//        public PostPaidOnlineCommandHandler(IBookingRepository bookingRepository, IVnPayService vnPayService, IParkingRepository parkingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
//        {
//            _bookingRepository = bookingRepository;
//            _vnPayService = vnPayService;
//            _parkingRepository = parkingRepository;
//            _userRepository = userRepository;
//            _businessProfileRepository = businessProfileRepository;
//        }
//        public async Task<ServiceResponse<string>> Handle(PostPaidOnlineCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                List<Expression<Func<Domain.Entities.Booking, object>>> includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
//                {
//                    x => x.ParkingSlot
//                };
//                var booking = await _bookingRepository.GetItemWithCondition(x => x.BookingId == request.BookingId, includes, false);
//                if (booking == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy đơn đặt.",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
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
//                if(manager == null)
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

//                if (booking.PaymentMethod.Equals(Domain.Enum.PaymentMethod.tra_sau.ToString()))
//                {
//                    BookingTransaction bt = new BookingTransaction
//                    {
//                        ParkingSlotName = booking.ParkingSlot.Name,
//                        TotalPrice = (decimal)booking.ActualPrice
//                    };
//                    var url = _vnPayService.CreatePaymentUrl(bt, businessAcc.VnPays.FirstOrDefault().TmnCode, businessAcc.VnPays.FirstOrDefault().HashSecret, request.context);
//                    booking.PaymentMethod = Domain.Enum.PaymentMethod.thanh_toan_online.ToString();
//                    await _bookingRepository.Save();
//                    return new ServiceResponse<string>
//                    {
//                        Data = url,
//                        Message = "Thành công",
//                        StatusCode = 201,
//                        Success = true
//                    };
//                }
//                return new ServiceResponse<string>
//                {
//                    Message = "Trạng thái của đơn không hợp lệ để thực hiện chức năng.",
//                    StatusCode = 400,
//                    Success = false
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

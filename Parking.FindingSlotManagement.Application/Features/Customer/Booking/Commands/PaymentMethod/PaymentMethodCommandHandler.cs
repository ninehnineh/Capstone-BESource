//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PaymentMethod
//{
//    public class PaymentMethodCommandHandler : IRequestHandler<PaymentMethodCommand, ServiceResponse<string>>
//    {
//        private readonly IBookingRepository _bookingRepository;

//        public PaymentMethodCommandHandler(IBookingRepository bookingRepository)
//        {
//            _bookingRepository = bookingRepository;
//        }

//        public async Task<ServiceResponse<string>> Handle(PaymentMethodCommand request, CancellationToken cancellationToken)
//        {
//            var bookingId = request.BookingId;
//            var paymentMethod = request.PaymentMethod;
//            try
//			{
//                var booking = await _bookingRepository
//                    .GetItemWithCondition(x => x.BookingId == bookingId, null, false);

//                if (booking == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Đơn đặt không hợp lệ.",
//                        Success = false,
//                        StatusCode = 200,
//                    };
//                }

//                if (booking.PaymentMethod == Domain.Enum.PaymentMethod.tra_sau.ToString())
//                {
//                    booking.PaymentMethod = paymentMethod;
//                    await _bookingRepository.Save();
//                }

//                return new ServiceResponse<string>
//                {
//                    Message = "Thành công",
//                    StatusCode = 200,
//                    Success = true,
//                };
//            }
//			catch (Exception ex)
//			{
//				throw new Exception(ex.Message);
//			}
//        }
//    }
//}

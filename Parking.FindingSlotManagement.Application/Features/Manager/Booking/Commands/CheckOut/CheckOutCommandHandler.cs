//using MediatR;
//using Microsoft.Extensions.Configuration;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Models.PushNotification;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Parking.FindingSlotManagement.Domain.Enum;
//using System.Linq.Expressions;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut
//{
//    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, ServiceResponse<string>>
//    {
//        private readonly IBookingRepository _bookingRepository;
//        private readonly IParkingPriceRepository _parkingPriceRepository;
//        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
//        private readonly ITimelineRepository _timelineRepository;
//        private readonly IVehicleInfoRepository _vehicleInfoRepository;
//        private readonly IFireBaseMessageServices _fireBaseMessageServices;
//        private readonly IConfiguration _configuration;

//        public CheckOutCommandHandler(IBookingRepository bookingRepository,
//            IParkingPriceRepository parkingPriceRepository,
//            IParkingHasPriceRepository parkingHasPriceRepository,
//            ITimelineRepository timelineRepository,
//            IVehicleInfoRepository vehicleInfoRepository,
//            IFireBaseMessageServices fireBaseMessageServices,
//            IConfiguration configuration)
//        {
//            _bookingRepository = bookingRepository;
//            _parkingPriceRepository = parkingPriceRepository;
//            _parkingHasPriceRepository = parkingHasPriceRepository;
//            _timelineRepository = timelineRepository;
//            _vehicleInfoRepository = vehicleInfoRepository;
//            _fireBaseMessageServices = fireBaseMessageServices;
//            _configuration = configuration;
//        }

//        public async Task<ServiceResponse<string>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
//        {
//            var checkOutTime = DateTime.UtcNow.AddHours(7);
//            var parkingId = request.ParkingId;


//            try
//            {
//                var booking = await _bookingRepository
//                    .GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);
//                var vehicleInfoId = booking.VehicleInforId;
//                if (booking == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Đơn đặt không tồn tại",
//                        StatusCode = 200,
//                        Success = false
//                    };
//                }

//                var bookingEndtime = booking.EndTime;

//                booking.CheckoutTime = checkOutTime;

//                await _bookingRepository.Save();


//                var checkInTimeHour = booking.CheckinTime.Value.Date.AddHours(booking.CheckinTime.Value.Hour);

//                var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
//                {
//                    x => x.ParkingPrice!,
//                    x => x.ParkingPrice!.Traffic!
//                };

//                var parkingHasPrice = await _parkingHasPriceRepository
//                        .GetAllItemWithCondition(x => x.ParkingId == parkingId, includes);

//                var vehicle = await _vehicleInfoRepository.GetById(vehicleInfoId);
//                var trafficId = vehicle.TrafficId;

//                var appliedParkingPriceId = parkingHasPrice
//                    .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficId)
//                    .FirstOrDefault()!.ParkingPriceId;

//                var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId!);

//                var timeLines = await _timelineRepository
//                    .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);
                

//                var penaltyPriceStepTime = parkingPrice.PenaltyPriceStepTime;
//                var penaltyPrice = parkingPrice.PenaltyPrice;

//                if (checkOutTime <= bookingEndtime)
//                {
//                    booking.Status = BookingStatus.Check_Out.ToString();
//                    booking.ActualPrice = booking.TotalPrice;
//                    if (booking.PaymentMethod.Equals(Domain.Enum.PaymentMethod.thanh_toan_online.ToString()))
//                    {
//                        booking.Status = BookingStatus.Done.ToString();
//                        await _bookingRepository.Save();
//                        return new ServiceResponse<string>
//                        {
//                            StatusCode = 204,
//                            Message = "Thành công",
//                            Success = true,
//                        };
//                    }
//                    else
//                    {
//                        await _bookingRepository.Save();
//                    }

//                }
//                else if (checkOutTime > bookingEndtime)
//                {
//                    //actualPrice += (decimal)penaltyPrice;
//                    var actualPriceLate = booking.TotalPrice;
//                    if (booking.PaymentMethod.Equals(Domain.Enum.PaymentMethod.thanh_toan_online.ToString()))
//                    {
//                        actualPriceLate = 0;
//                    }

//                    if (penaltyPriceStepTime == 0)
//                    {
//                        actualPriceLate += (decimal)penaltyPrice;
//                    }
//                    else
//                    {
//                        if ((checkOutTime - bookingEndtime) <= TimeSpan.FromHours((double)penaltyPriceStepTime))
//                        {
//                            actualPriceLate += (decimal)penaltyPrice;
//                        }
//                        else
//                        {
//                            actualPriceLate += (decimal)penaltyPrice;
//                            var penaltyTime = checkOutTime.Hour - bookingEndtime.Value.Hour;
//                            var step = penaltyTime / penaltyPriceStepTime;
//                            actualPriceLate += ((decimal)step * (decimal)penaltyPrice);
//                        }
//                    }

//                    booking.Status = BookingStatus.Check_Out.ToString();
//                    booking.ActualPrice = actualPriceLate;
//                    await _bookingRepository.Save();
//                }

//                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
//                    .GetSection("CheckOut").Value;
//                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
//                    .GetSection("CheckOut").Value;
//                var userDiviceToken = booking.User!.Devicetoken;

//                var pushNotificationMobile = new PushNotificationMobileModel
//                {
//                    Title = titleCustomer,
//                    Message = bodyCustomer,
//                    TokenMobile = userDiviceToken,
//                };

//                await _fireBaseMessageServices
//                    .SendNotificationToMobileAsync(pushNotificationMobile);

//                return new ServiceResponse<string>
//                {
//                    StatusCode = 204,
//                    Message = "Thành công",
//                    Success = true,
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//    }
//}

using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System.Linq.Expressions;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IConfiguration _configuration;

        public CheckOutCommandHandler(IBookingRepository bookingRepository, 
            IParkingPriceRepository parkingPriceRepository,
            IParkingHasPriceRepository parkingHasPriceRepository,
            ITimelineRepository timelineRepository,
            IVehicleInfoRepository vehicleInfoRepository, 
            IFireBaseMessageServices fireBaseMessageServices,
            IConfiguration configuration)
        {
            _bookingRepository = bookingRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _timelineRepository = timelineRepository;
            _vehicleInfoRepository = vehicleInfoRepository;
            _fireBaseMessageServices = fireBaseMessageServices;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            var checkOutTime = DateTime.UtcNow.AddHours(7);
            var parkingId = request.ParkingId;
            var vehicleInfoId = request.VehicleInfoId;

            try
            {
                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đặt không tồn tại",
                        StatusCode = 200,
                        Success = false
                    };
                }

                var bookingEndtime = booking.EndTime;

                booking.CheckoutTime = checkOutTime;

                await _bookingRepository.Save();

                var checkInTimeHour = booking.CheckinTime.Value.Date.AddHours(booking.CheckinTime.Value.Hour);

                var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice!,
                    x => x.ParkingPrice!.Traffic!
                };

                var parkingHasPrice = await _parkingHasPriceRepository
                        .GetAllItemWithCondition(x => x.ParkingId == parkingId, includes);

                var vehicle = await _vehicleInfoRepository.GetById(vehicleInfoId);
                var trafficId = vehicle.TrafficId;

                var appliedParkingPriceId = parkingHasPrice
                    .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficId)
                    .FirstOrDefault()!.ParkingPriceId;

                var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId!);

                var timeLines = await _timelineRepository
                    .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);

                var actualPrice = CaculateActualPrice(checkInTimeHour, checkOutTime,
                    parkingPrice, timeLines);

                var penaltyPriceStepTime = parkingPrice.PenaltyPriceStepTime;
                var penaltyPrice = parkingPrice.PenaltyPrice;

                if (checkOutTime <= bookingEndtime)
                {
                    booking.Status = BookingStatus.Check_Out.ToString();
                    booking.ActualPrice = actualPrice;
                    await _bookingRepository.Save();
                }
                else if (checkOutTime > bookingEndtime)
                {
                    //actualPrice += (decimal)penaltyPrice;

                    if (penaltyPriceStepTime == 0)
                    {
                        actualPrice += (decimal) penaltyPrice;
                    }
                    else
                    {
                        if ((checkOutTime - bookingEndtime) <= TimeSpan.FromHours((double)penaltyPriceStepTime))
                        {
                            actualPrice += (decimal)penaltyPrice;
                        }
                        else
                        {
                            actualPrice += (decimal)penaltyPrice;
                            var penaltyTime = checkOutTime.Hour - bookingEndtime.Value.Hour;
                            var step = penaltyTime / penaltyPriceStepTime;
                            actualPrice += ((decimal)step * (decimal)penaltyPrice);
                        }
                    }

                    booking.Status = BookingStatus.Check_Out.ToString();
                    booking.ActualPrice = actualPrice;
                    await _bookingRepository.Save();
                }

                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                    .GetSection("CheckOut").Value;
                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                    .GetSection("CheckOut").Value;
                var userDiviceToken = booking.User!.Devicetoken;

                var pushNotificationMobile = new PushNotificationMobileModel
                {
                    Title = titleCustomer,
                    Message = bodyCustomer,
                    TokenMobile = userDiviceToken,
                };

                await _fireBaseMessageServices
                    .SendNotificationToMobileAsync(pushNotificationMobile);

                return new ServiceResponse<string>
                {
                    StatusCode = 204,
                    Message = "Thành công",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static decimal CaculateActualPrice(DateTime checkInTime, DateTime checkOutTime
            , Domain.Entities.ParkingPrice parkingPrice, IEnumerable<TimeLine> timeLines)
        {
            var startTimeBooking = checkInTime.TimeOfDay.TotalHours;
            var endTimeBooking = checkOutTime.TimeOfDay.TotalHours;
            var startTimeDate = checkInTime.Date;
            var endTimeDate = checkOutTime.Date;
            var startingTime = parkingPrice.StartingTime;
            var extraTimeStep = parkingPrice.ExtraTimeStep;
            bool foundStartPoint = false;
            var hitEndPoint = false;
            var totalPrice = 0M;
            var startingPoint = 0;
            var extraFeePoint = 0;
            var isPass = false;

            foreach (var package in timeLines)
            {
                var startTimePackage = package.StartTime?.TotalHours;
                var endTimePackage = package.EndTime?.TotalHours;
                if (startTimeDate == endTimeDate)
                {
                    // một gói, trong ngày
                    if (startTimePackage > endTimePackage)
                    {
                        endTimePackage += 24;
                    }
                    if (startTimeBooking >= startTimePackage &&
                        startTimeBooking <= endTimePackage &&
                        endTimeBooking > startTimeBooking &&
                        endTimeBooking >= startTimePackage &&
                        endTimeBooking <= endTimePackage)
                    {
                        if (package.StartTime > package.EndTime)
                        {
                            if (Math.Abs(24 - startTimeBooking) >= 24 - startTimePackage &&
                                Math.Abs(24 - endTimeBooking) >= 24 - startTimePackage)
                            {
                                startTimeBooking += 24;
                                endTimeBooking += 24;
                            }
                            package.EndTime += TimeSpan.FromHours(24);
                        }

                        if (startTimeBooking >= startTimePackage &&
                            startTimeBooking <= endTimePackage &&
                            endTimeBooking >= startTimePackage &&
                            endTimeBooking <= endTimePackage)
                        {
                            var so_tieng_book = (decimal)(endTimeBooking - startTimeBooking);
                            if (startingTime < so_tieng_book)
                            {
                                var so_tieng_tinh_ExtraFee = so_tieng_book - startingTime;
                                var so_step = (int)so_tieng_tinh_ExtraFee / (int)extraTimeStep!;
                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee) + (decimal)package.ExtraFee;
                            }
                            else
                            {
                                totalPrice = package.Price;
                            }
                        }

                        if (endTimeBooking > 24 &&
                            startTimeBooking > 24)
                        {
                            endTimeBooking -= 24;
                            startTimeBooking -= 24;
                        }
                        break;
                    }

                    // nhiều gói, trong ngày
                    else
                    {
                        while (hitEndPoint == false)
                        {
                            if (endTimePackage > 24 && startTimeBooking < 24)
                            {
                                startTimeBooking += 24;
                            }
                            if (startTimeBooking >= startTimePackage &&
                                startTimeBooking < endTimePackage &&
                                foundStartPoint == false)
                            {
                                foundStartPoint = true;
                                var step = 0;
                                var startingPrice = 0M;

                                startingPrice =
                                    (double)(startTimeBooking + startingTime)! == endTimePackage
                                    ? package.Price
                                    : package.Price + (decimal)package.ExtraFee!;

                                var extraPrice = 0M;
                                startingPoint = (int)(startTimeBooking + startingTime)!;
                                if (startingPoint == endTimePackage)
                                {
                                    isPass = true;
                                }
                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;
                                while (extraFeePoint <= endTimePackage)
                                {
                                    step++;
                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
                                    extraPrice = step * (decimal)package.ExtraFee!;
                                };
                                totalPrice += startingPrice + extraPrice;

                                break;
                            }

                            if (foundStartPoint == true)
                            {
                                var priceOfTimeLineTwo = 0M;
                                if (startingPoint == startTimePackage ||
                                    startingPoint == startTimePackage + 24)
                                {
                                    totalPrice += (decimal)package.ExtraFee!;
                                }
                                if (extraFeePoint > 24 && endTimeBooking < 24)
                                {
                                    endTimeBooking += 24;
                                }
                                while (extraFeePoint <= endTimeBooking)
                                {
                                    priceOfTimeLineTwo = (decimal)package.ExtraFee!;
                                    totalPrice += priceOfTimeLineTwo;
                                    extraFeePoint += (int)extraTimeStep!;
                                    if (endTimePackage < startTimePackage)
                                    {
                                        endTimePackage += 24;
                                    }
                                    if (extraFeePoint > 24 && endTimePackage < 24)
                                    {
                                        endTimePackage += 24;
                                    }
                                    if (extraFeePoint > endTimePackage)
                                    {
                                        if (extraFeePoint > 24)
                                        {
                                            extraFeePoint -= 24;
                                            endTimeBooking -= 24;
                                            package.EndTime -= TimeSpan.FromHours(24);
                                        }
                                        break;
                                    }
                                }
                                if (extraFeePoint > endTimeBooking)
                                {
                                    isPass = false;
                                    break;
                                }
                                else break;
                            }
                            if (startTimeBooking > 24)
                            {
                                startTimeBooking -= 24;
                            }

                            if (extraFeePoint > endTimeBooking && isPass == false)
                            {
                                hitEndPoint = true;
                                break;
                            }
                            break;
                        }
                    }
                }

                if (startTimeDate < endTimeDate)
                {
                    if (endTimeBooking == 0)
                    {
                        endTimeBooking += 24;
                    }
                    if (startTimePackage > endTimePackage)
                    {
                        endTimePackage += 24;
                    }
                    // một gói, qua ngày
                    if (startTimeBooking >= startTimePackage &&
                        startTimeBooking < endTimePackage &&
                        endTimeBooking > startTimeBooking &&
                        endTimeBooking >= startTimePackage &&
                        endTimeBooking <= endTimePackage)
                    {
                        if (package.StartTime > package.EndTime)
                        {
                            if (startTimeBooking > endTimeBooking)
                            {
                                endTimeBooking += 24;
                            }
                            package.EndTime += TimeSpan.FromHours(24);
                        }

                        if (startTimeBooking >= startTimePackage &&
                            startTimeBooking < endTimePackage &&
                            endTimeBooking >= startTimePackage &&
                            endTimeBooking <= endTimePackage)
                        {
                            var so_tieng_book = (decimal)(endTimeBooking - startTimeBooking);
                            if (startingTime < so_tieng_book)
                            {
                                var so_tieng_tinh_ExtraFee = so_tieng_book - startingTime;
                                var so_step = (int)so_tieng_tinh_ExtraFee / (int)extraTimeStep!;
                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee!) + (decimal)package.ExtraFee!;
                            }
                            else
                            {
                                totalPrice = package.Price;
                            }
                        }

                        if (endTimeBooking > 24)
                        {
                            endTimeBooking -= 24;
                        }
                        break;
                    }

                    // nhiều gói, qua ngày
                    else
                    {
                        while (hitEndPoint == false)
                        {
                            if (endTimePackage < startTimePackage)
                            {
                                package.EndTime += TimeSpan.FromHours(24);
                            }

                            if (startTimeBooking >= startTimePackage &&
                                startTimeBooking < endTimePackage &&
                                foundStartPoint == false)
                            {
                                foundStartPoint = true;
                                var step = 0;
                                var startingPrice = 0M;

                                startingPrice =
                                    (double)(startTimeBooking + startingTime)! == endTimePackage
                                    ? package.Price
                                    : package.Price + (decimal)package.ExtraFee!;

                                var extraPrice = 0M;
                                var bookedTime = (int)(endTimePackage - startTimeBooking);
                                startingPoint = (int)(startTimeBooking + startingTime!);

                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;
                                while (extraFeePoint <= endTimePackage)
                                {
                                    step++;
                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
                                    extraPrice = step * (decimal)package.ExtraFee!;
                                };
                                totalPrice = startingPrice + extraPrice;

                                break;
                            }

                            if (foundStartPoint)
                            {
                                if (startingPoint == startTimePackage)
                                {
                                    totalPrice += (decimal)package.ExtraFee!;
                                }
                                while (extraFeePoint <= endTimeBooking)
                                {
                                    totalPrice += (decimal)package.ExtraFee!;
                                    extraFeePoint += (int)extraTimeStep!;
                                    if (endTimePackage < startTimePackage)
                                    {
                                        endTimePackage += 24;
                                    }
                                    if (extraFeePoint > 24 && endTimePackage < 24)
                                    {
                                        endTimePackage += 24;
                                    }
                                    if (extraFeePoint > endTimePackage)
                                    {
                                        if (extraFeePoint > 24)
                                        {
                                            extraFeePoint -= 24;
                                            endTimeBooking -= 24;
                                            package.EndTime -= TimeSpan.FromHours(24);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (extraFeePoint >= endTimeBooking)
                            {
                                hitEndPoint = true;
                                break;
                            }
                            break;
                        }
                    }
                }
            }

            return totalPrice;
        }
    }
}

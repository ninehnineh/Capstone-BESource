using AutoMapper;
using Hangfire.States;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.BookingInformation
{
    public class BookingInformationCommandHandler : IRequestHandler<BookingInformationCommand, ServiceResponse<BookingInformationResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;

        public BookingInformationCommandHandler(IBookingRepository bookingRepository,
            IMapper mapper,
            ITransactionRepository transactionRepository,
            ITimelineRepository timelineRepository,
            IParkingHasPriceRepository parkingHasPriceRepository,
            IParkingPriceRepository parkingPriceRepository,
            IVehicleInfoRepository vehicleInfoRepository,
            ITimeSlotRepository timeSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _timelineRepository = timelineRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _vehicleInfoRepository = vehicleInfoRepository;
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<ServiceResponse<BookingInformationResponse>> Handle(BookingInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var expressions = new List<Expression<Func<Booking, object>>>()
                //{
                //    x => x.BookingDetails.First().TimeSlot.Parkingslot.Floor,
                //    x => x.Transactions,
                //    x => x.VehicleInfor
                //};

                var booking = await _bookingRepository
                    .GetBookingInclude(request.BookingId);


                if (booking == null)
                {
                    return new ServiceResponse<BookingInformationResponse>
                    {
                        Message = "Đơn đặt không tồn tại",
                        StatusCode = 200,
                        Success = false
                    };
                }
                if (booking.Status.Equals(BookingStatus.Success.ToString()) || booking.Status.Equals(BookingStatus.Cancel.ToString()))
                {
                    return new ServiceResponse<BookingInformationResponse>
                    {
                        Message = "Đơn chưa check-in hoặc đã bị hủy nên không thể xử lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var parkingId = booking.BookingDetails.First().TimeSlot.Parkingslot.Floor.ParkingId;
                if (booking.Status.Equals(BookingStatus.Check_Out.ToString()) || booking.Status.Equals(BookingStatus.Done.ToString()))
                {
                    var response2 = new BookingInformationResponse
                    {
                        ParkingId = (int)parkingId,
                        Booking = _mapper.Map<BookingCheckoutDto>(booking),
                    };

                    return new ServiceResponse<BookingInformationResponse>
                    {
                        Data = response2,
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 201
                    };
                }
                else if (booking.Status.Equals(BookingStatus.Check_In.ToString()) || booking.Status.Equals(BookingStatus.OverTime.ToString()))
                {
                    var endTimeBooking = booking.EndTime.Value;
                    var startTimeBooking = booking.StartTime;
                    var checkinTime = booking.CheckinTime;

                    var checkOutTime = DateTime.UtcNow.AddHours(7);
                    booking.CheckoutTime = checkOutTime;


                    var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
                    {
                        x => x.ParkingPrice!,
                        x => x.ParkingPrice!.Traffic!
                    };

                    var parkingHasPrice = await _parkingHasPriceRepository
                            .GetAllItemWithCondition(x => x.ParkingId == parkingId, includes);

                    var vehicle = await _vehicleInfoRepository.GetById(booking.VehicleInforId);
                    var trafficId = vehicle.TrafficId;

                    var appliedParkingPriceId = parkingHasPrice
                        .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficId)
                        .FirstOrDefault()!.ParkingPriceId;

                    var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId!);

                    var timeLines = await _timelineRepository
                        .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);

                    var penaltyPriceStepTime = parkingPrice.PenaltyPriceStepTime;
                    if (penaltyPriceStepTime == null)
                    {
                        return new ServiceResponse<BookingInformationResponse>
                        {
                            Message = "Con chó link quên nhập penaltyPriceStepTime của parking price",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                    var penaltyPrice = parkingPrice.PenaltyPrice;

                    // vào đúng giờ, ra đúng giờ
                    if (checkOutTime <= endTimeBooking && checkinTime >= startTimeBooking)
                    {
                        if (booking.Transactions.First().Status == BookingPaymentStatus.Chua_thanh_toan.ToString() &&
                            booking.Transactions.First().PaymentMethod == PaymentMethod.tra_sau.ToString())
                        {
                            booking.UnPaidMoney += booking.Transactions.First().Price;
                        }
                    }

                    // vào đúng giờ, ra tre
                    else if (checkOutTime > endTimeBooking && checkinTime >= startTimeBooking)
                    {
                        // tính tiền ra trễ
                        if (checkOutTime - endTimeBooking > TimeSpan.FromMinutes(15))
                        {
                            var actualPriceLate = 0M;
                            if (penaltyPriceStepTime == 0)
                            {
                                actualPriceLate += (decimal)penaltyPrice;
                            }
                            else
                            {
                                if ((checkOutTime - endTimeBooking) <= TimeSpan.FromHours((double)penaltyPriceStepTime))
                                {
                                    actualPriceLate += (decimal)penaltyPrice;
                                }
                                else
                                {
                                    actualPriceLate += (decimal)penaltyPrice;
                                    var penaltyTime = checkOutTime - endTimeBooking;
                                    var step = penaltyTime.Hours / (int)penaltyPriceStepTime;
                                    actualPriceLate += (step * (decimal)penaltyPrice);
                                }
                            }

                            Transaction bp = new Transaction
                            {
                                Price = actualPriceLate,
                                Status = Domain.Enum.BookingPaymentStatus.Chua_thanh_toan.ToString(),
                                PaymentMethod = Domain.Enum.PaymentMethod.tra_sau.ToString(),
                                Description = "Phí phạt ra bãi trễ",
                                BookingId = booking.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                            };

                            await _transactionRepository.Insert(bp);
                        }


                        foreach (var item in booking.Transactions)
                        {
                            if (item.Status == BookingPaymentStatus.Chua_thanh_toan.ToString())
                            {
                                booking.UnPaidMoney += item.Price;
                            }
                        }
                    }

                    // vao som, ra dung
                    else if (checkOutTime <= endTimeBooking && checkinTime < startTimeBooking)
                    {
                        if (startTimeBooking - checkinTime > TimeSpan.FromMinutes(15))
                        {
                            var checkinTimeHour = checkinTime.Value.Hour;
                            var earlyTimeHour = startTimeBooking.Hour - checkinTimeHour;
                            var money = 0M;
                            foreach (var item in timeLines)
                            {
                                if (item.StartTime <= TimeSpan.FromHours(checkinTime.Value.Hour) &&
                                    TimeSpan.FromHours(checkinTime.Value.Hour) < item.EndTime)
                                {
                                    money += (decimal)item.ExtraFee * earlyTimeHour;
                                }
                                else if (item.StartTime <= TimeSpan.FromHours(checkinTime.Value.Hour) &&
                                    TimeSpan.FromHours(checkinTime.Value.Hour) > item.EndTime && item.StartTime > item.EndTime)
                                {
                                    money += (decimal)item.ExtraFee * earlyTimeHour;
                                }
                            }

                            Transaction bp = new Transaction
                            {
                                Price = money,
                                Status = Domain.Enum.BookingPaymentStatus.Chua_thanh_toan.ToString(),
                                PaymentMethod = Domain.Enum.PaymentMethod.tra_sau.ToString(),
                                Description = "Phí vào sớm hơn dự kiến",
                                BookingId = booking.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                            };

                            await _transactionRepository.Insert(bp);

                        }

                        foreach (var item in booking.Transactions)
                        {
                            if (item.Status == BookingPaymentStatus.Chua_thanh_toan.ToString())
                            {
                                booking.UnPaidMoney += item.Price;
                            }
                        }

                    }

                    // vao som, ra tre
                    else if (checkOutTime > endTimeBooking && checkinTime < startTimeBooking)
                    {
                        if (startTimeBooking - checkinTime > TimeSpan.FromMinutes(15))
                        {
                            var checkinTimeHour = checkinTime.Value.Hour;
                            var earlyTimeHour = startTimeBooking.Hour - checkinTimeHour;
                            var money = 0M;
                            foreach (var item in timeLines)
                            {
                                if (item.StartTime <= TimeSpan.FromHours(checkinTime.Value.Hour) &&
                                    TimeSpan.FromHours(checkinTime.Value.Hour) < item.EndTime)
                                {
                                    money += (decimal)item.ExtraFee * earlyTimeHour;
                                }
                                else if (item.StartTime <= TimeSpan.FromHours(checkinTime.Value.Hour) &&
                                    TimeSpan.FromHours(checkinTime.Value.Hour) > item.EndTime && item.StartTime > item.EndTime)
                                {
                                    money += (decimal)item.ExtraFee * earlyTimeHour;
                                }
                            }

                            Transaction bp = new Transaction
                            {
                                Price = money,
                                Status = Domain.Enum.BookingPaymentStatus.Chua_thanh_toan.ToString(),
                                PaymentMethod = Domain.Enum.PaymentMethod.tra_sau.ToString(),
                                Description = "Phí vào sớm hơn dự kiến",
                                BookingId = booking.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                            };

                            await _transactionRepository.Insert(bp);



                        }

                        // tính tiền ra trễ
                        if (checkOutTime - endTimeBooking > TimeSpan.FromMinutes(15))
                        {
                            var actualPriceLate = 0M;
                            if (penaltyPriceStepTime == 0)
                            {
                                actualPriceLate += (decimal)penaltyPrice;
                            }
                            else
                            {
                                if ((checkOutTime - endTimeBooking) <= TimeSpan.FromHours((double)penaltyPriceStepTime))
                                {
                                    actualPriceLate += (decimal)penaltyPrice;
                                }
                                else
                                {
                                    actualPriceLate += (decimal)penaltyPrice;
                                    var penaltyTime = checkOutTime - endTimeBooking;
                                    var step = penaltyTime.Hours / (int)penaltyPriceStepTime;
                                    actualPriceLate += (step * (decimal)penaltyPrice);
                                }
                            }

                            Transaction bp1 = new Transaction
                            {
                                Price = actualPriceLate,
                                Status = Domain.Enum.BookingPaymentStatus.Chua_thanh_toan.ToString(),
                                PaymentMethod = Domain.Enum.PaymentMethod.tra_sau.ToString(),
                                Description = "Phí phạt ra bãi trễ",
                                BookingId = booking.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                            };

                            await _transactionRepository.Insert(bp1);
                        }

                        foreach (var item in booking.Transactions)
                        {
                            if (item.Status == BookingPaymentStatus.Chua_thanh_toan.ToString())
                            {
                                booking.UnPaidMoney += item.Price;
                            }
                        }
                    }

                    foreach (var bookingDetail in booking.BookingDetails)
                    {
                        bookingDetail.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                    }

                    await _timeSlotRepository.Save();

                    booking.Status = BookingStatus.Check_Out.ToString();
                    await _bookingRepository.Save();

                    var response = new BookingInformationResponse
                    {
                        ParkingId = (int)parkingId,
                        Booking = _mapper.Map<BookingCheckoutDto>(booking),
                    };

                    return new ServiceResponse<BookingInformationResponse>
                    {
                        Data = response,
                        Message = "Thành công",
                        Success = true,
                        StatusCode = 201
                    };
                }

                return new ServiceResponse<BookingInformationResponse>
                {
                    Message = "Có lỗi xảy ra.",
                    Success = false,
                    StatusCode = 400
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

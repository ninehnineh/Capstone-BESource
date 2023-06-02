//using AutoMapper;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using Org.BouncyCastle.Utilities;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Domain.Enum;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands
//{
//    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ServiceResponse<string>>
//    {
//        private readonly IBookingRepository _bookingRepository;
//        private readonly IParkingSlotRepository _parkingSlotRepository;
//        private readonly ITrafficRepository _trafficRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly IMapper _mapper;
//        private readonly IFloorRepository _floorRepository;
//        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
//        private readonly IParkingRepository _parkingRepository;
//        private readonly IVnPayRepository _vnPayRepository;
//        private readonly ITimelineRepository _timelineRepository;
//        private readonly ILogger<CreateBookingCommandHandler> _logger;
//        private readonly IParkingPriceRepository _parkingPriceRepository;

//        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
//            IParkingSlotRepository parkingSlotRepository,
//            ITrafficRepository trafficRepository,
//            IUserRepository userRepository, 
//            IMapper mapper,
//            IFloorRepository floorRepository,
//            IParkingHasPriceRepository parkingHasPriceRepository,
//            IParkingRepository parkingRepository,
//            IVnPayRepository vnPayRepository, 
//            ITimelineRepository timelineRepository,
//            ILogger<CreateBookingCommandHandler> logger,
//            IParkingPriceRepository parkingPriceRepository)
//        {
//            _bookingRepository = bookingRepository;
//            _parkingSlotRepository = parkingSlotRepository;
//            _trafficRepository = trafficRepository;
//            _userRepository = userRepository;
//            _mapper = mapper;
//            _floorRepository = floorRepository;
//            _parkingHasPriceRepository = parkingHasPriceRepository;
//            _parkingRepository = parkingRepository;
//            _vnPayRepository = vnPayRepository;
//            _timelineRepository = timelineRepository;
//            _logger = logger;
//            _parkingPriceRepository = parkingPriceRepository;
//        }

//        public async Task<ServiceResponse<string>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var parkingSlot = await _parkingSlotRepository.GetById(request.ParkingSlotId);
//                if (parkingSlot == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Chỗ để xe không khả dụng",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                _logger.LogInformation($"parking slot {parkingSlot}");
//                var vehicleInfor = await _trafficRepository.GetById(request.VehicleInforId);
//                if (vehicleInfor == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Loại xe không tồn tại",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                _logger.LogInformation($"vehicleInfor: {vehicleInfor}");
//                var user = await _userRepository
//                    .GetItemWithCondition(x => x.UserId == request.UserId &&
//                                            x.IsActive == true &&
//                                            x.RoleId == 3);
//                if (user == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Người dùng không tồn tại",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                _logger.LogInformation($"user: {user}");
//                var entity = _mapper.Map<Domain.Entities.Booking>(request);
//                entity.Status = BookingStatus.Initial.ToString();
//                _logger.LogInformation($"status {BookingStatus.Initial.ToString()}");
//                // get and set TMNCodeVnPay
//                var floor = await _floorRepository.GetById(parkingSlot.FloorId!);
//                var parkingId = floor.ParkingId;
//                _logger.LogInformation($"parking id: {parkingId}");

//                var parking = await _parkingRepository.GetById(parkingId!);
//                var managerId = parking.ManagerId;
//                _logger.LogInformation($"manager id: {managerId}");

//                //var vnpay = await _vnPayRepository
//                //    .GetItemWithCondition(x => x.ManagerId == managerId);
//                //entity.TmnCodeVnPay = vnpay.TmnCode;

//                // Set TotalPrice

//                var trafficid = vehicleInfor.TrafficId;
//                _logger.LogInformation($"traffic id: {trafficid}");

//                if (parking.CarSpot <= 0 && trafficid == 1)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Bãi không giữ xe ô tô, vui lòng chọn bãi khác",
//                        Success = true,
//                        StatusCode = 200,
//                    };
//                }
//                else if (parking.MotoSpot <= 0 && trafficid == 2)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Bãi không giữ xe máy, vui lòng chọn bãi khác",
//                        Success = true,
//                        StatusCode = 200,
//                    };
//                }

//                var parkingHasPrice = await _parkingHasPriceRepository
//                        .GetAllItemWithCondition(x => x.ParkingId == parkingId);

//                _logger.LogInformation($"parking has price: {parkingHasPrice.Count()}");

//                var StartTimeBooking = request.StartTime;
//                _logger.LogInformation($"StartTimeBooking: {StartTimeBooking}");

//                var EndTimeBooking = request.EndTime;
//                _logger.LogInformation($"EndTimeBooking: {EndTimeBooking}");

//                //var parkingPrice = parkingHasPrice.park

//                var price = 0M;
//                var extraFee = 0M;
//                var penaltyPrice = 0M;
//                var count = 0;
//                TimeSpan resHours = new TimeSpan();
//                if (EndTimeBooking.Date > StartTimeBooking.Date)
//                {
//                    resHours = EndTimeBooking - StartTimeBooking;
//                }
//                else
//                {
//                    resHours = EndTimeBooking.TimeOfDay - StartTimeBooking.TimeOfDay;
//                }
//                var tong_gio = resHours.TotalHours;


//                foreach (var item in parkingHasPrice)
//                {
//                    var timeLines = await _timelineRepository
//                        .GetAllItemWithCondition(x => x.ParkingPriceId == item.ParkingPriceId);

//                    if (timeLines.FirstOrDefault().TrafficId != trafficid)
//                    {
//                        return new ServiceResponse<string>
//                        {
//                            Message = "Bãi không hỗ trợ loại phương tiện của bạn.",
//                            Success = false,
//                            StatusCode = 400,
//                        };
//                    }
                    
//                    foreach (var timeLine in timeLines)
//                    {
//                        /// 
//// Parking.isOver9 == true && ParkingPrice.IsStartAndEndNull == true
//                        if (parking.IsOvernight == true )
//                        {

//                        }


//                        // else if (parkingPrice.isStartAndEndNull == false)

//                        //if (StartTimeBooking.TimeOfDay >= timeLine.StartTime && EndTimeBooking.TimeOfDay <= timeLine.EndTime)
//                        //{
//                        //    var hours = 0D;
//                        //    if (EndTimeBooking > StartTimeBooking)
//                        //    {
//                        //        hours = (EndTimeBooking.TimeOfDay - StartTimeBooking.TimeOfDay).TotalHours;
//                        //        //return price;
//                        //    }
//                        //    else { 
//                        //        hours = ((EndTimeBooking.TimeOfDay + TimeSpan.FromHours(24)) - StartTimeBooking.TimeOfDay).TotalHours;
//                        //    }
//                        //    price = timeLine.Price * (decimal)hours;

//                        //}
//                        // giờ book nằm trong một timeLine của khung giờ
//                        if (StartTimeBooking.Date < EndTimeBooking.Date)
//                        {
//                            var tonggio = (decimal)(EndTimeBooking.TimeOfDay + TimeSpan.FromHours(24) - StartTimeBooking.TimeOfDay).TotalHours;
//                            var tonggiophi = tonggio - timeLine.StartingTime;
//                            price = timeLine.Price + (decimal)(tonggiophi * timeLine.ExtraFee);
//                            return new ServiceResponse<string>
//                            {
//                                Data = price.ToString(),
//                            };
//                        }
//                        else 
//                        { 
//                            if (StartTimeBooking.TimeOfDay >= timeLine.StartTime &&
//                                StartTimeBooking.TimeOfDay <= timeLine.EndTime &&
//                                EndTimeBooking.TimeOfDay > StartTimeBooking.TimeOfDay &&
//                                EndTimeBooking.TimeOfDay <= timeLine.EndTime)
//                            {
//                                var tonggio = (decimal) (EndTimeBooking.TimeOfDay.TotalHours - StartTimeBooking.TimeOfDay.TotalHours - timeLine.StartingTime);
//                                price = timeLine.Price + (tonggio * (decimal) timeLine.ExtraFee);
//                                return new ServiceResponse<string>
//                                {
//                                    Data = price.ToString(),
//                                };
//                            }
//                        }

//                        if (StartTimeBooking.TimeOfDay >= timeLine.StartTime &&
//                            StartTimeBooking.TimeOfDay <= timeLine.EndTime && 
//                            EndTimeBooking.TimeOfDay >= timeLine.EndTime)
//                        {
//                            var tieng_se_tru = timeLine.EndTime - StartTimeBooking.TimeOfDay;
//                            tong_gio -= tieng_se_tru.Value.TotalHours;
//                            price = price + (timeLine.Price * (decimal) tieng_se_tru.Value.TotalHours);
//                            if (timeLine.IsExtrafee == true)
//                            {
//                                if (tieng_se_tru.Value.TotalHours > timeLine.StartingTime)
//                                {
//                                    extraFee =  extraFee + (decimal) ((decimal)(tieng_se_tru.Value.TotalHours - timeLine.StartingTime) * timeLine.ExtraFee);
//                                    _logger.LogInformation($"extraFee: {extraFee}");
//                                }
//                            }
//                        }

//                        if (timeLine.StartTime > timeLine.EndTime)
//                        {
//                            if (EndTimeBooking.Date > StartTimeBooking.Date)
//                            {
//                                TimeSpan a = TimeSpan.FromHours(24) + (TimeSpan)EndTimeBooking.TimeOfDay;
//                                TimeSpan b = TimeSpan.FromHours(24) + (TimeSpan)timeLine.EndTime;
//                                if (a >= timeLine.StartTime &&
//                                    a <= b)
//                                {
//                                    var tieng_se_tru = a - timeLine.StartTime;
//                                    tong_gio -= tieng_se_tru.Value.TotalHours;
//                                    price = price + (timeLine.Price * (decimal)tieng_se_tru.Value.TotalHours);
//                                    if (timeLine.IsExtrafee == true)
//                                    {
//                                        extraFee = extraFee + (decimal)((decimal)(tieng_se_tru.Value.TotalHours) * timeLine.ExtraFee!);
//                                        _logger.LogInformation($"extraFee: {extraFee}");

//                                    }
//                                }
//                            }
//                            if (EndTimeBooking.TimeOfDay >= timeLine.StartTime &&
//                                EndTimeBooking.TimeOfDay >= timeLine.EndTime)
//                            {
//                                var tieng_se_tru = EndTimeBooking.TimeOfDay - timeLine.StartTime;
//                                tong_gio -= tieng_se_tru.Value.TotalHours;
//                                price = price + (timeLine.Price * (decimal)tieng_se_tru.Value.TotalHours);
//                                if (timeLine.IsExtrafee == true)
//                                {
//                                    extraFee = extraFee + (decimal)((decimal)(tieng_se_tru.Value.TotalHours) * timeLine.ExtraFee);
//                                    _logger.LogInformation($"extraFee: {extraFee}");

//                                }
//                            }
//                        }

//                        else {
                        
//                            if (EndTimeBooking.TimeOfDay >= timeLine.StartTime &&
//                                EndTimeBooking.TimeOfDay <= timeLine.EndTime)
//                            {
//                                var tieng_se_tru = EndTimeBooking.TimeOfDay - timeLine.StartTime;
//                                tong_gio -= tieng_se_tru.Value.TotalHours;
//                                price = price + (timeLine.Price * (decimal)tieng_se_tru.Value.TotalHours);
//                                if (timeLine.IsExtrafee == true)
//                                {
//                                    extraFee = extraFee + (decimal)((decimal)(tieng_se_tru.Value.TotalHours) * timeLine.ExtraFee);
//                                    _logger.LogInformation($"extraFee: {extraFee}");

//                                }
//                            }
//                        }

//                        if (timeLine.StartTime > timeLine.EndTime)
//                        {

//                            var tieng_se_tru = (timeLine.EndTime - timeLine.StartTime) * (-1);
//                            var endtime_qua_ngay = timeLine.StartTime + tieng_se_tru;
//                            //if (endtime_qua_ngay > EndTimeBooking.TimeOfDay)
//                            //{

//                            //}
//                        }
//                        else
//                        {
//                            if (StartTimeBooking.TimeOfDay <= timeLine.StartTime &&
//                                EndTimeBooking.TimeOfDay >= timeLine.EndTime &&
//                                EndTimeBooking.TimeOfDay >= timeLine.StartTime)
//                            {
//                                var tieng_se_tru = timeLine.EndTime - timeLine.StartTime;
//                                tong_gio -= tieng_se_tru.Value.TotalHours;
//                                price = price + (timeLine.Price * (decimal)tieng_se_tru.Value.TotalHours);
//                                if (timeLine.IsExtrafee == true)
//                                {
//                                    if (tieng_se_tru.Value.TotalHours > timeLine.StartingTime)
//                                    {
//                                        extraFee = extraFee + (decimal)((decimal)(tieng_se_tru.Value.TotalHours) * timeLine.ExtraFee);
//                                        _logger.LogInformation($"extraFee: {extraFee}");

//                                    }
//                                }
//                            }
//                        }
////
//                    }
//                }

//                var x = price;
//                _logger.LogInformation($"price: {price}");
//                var y = extraFee;
//                _logger.LogInformation($"price: {y}");

//                return null;

//            }
//            catch (Exception ex)
//            {
//                if (ex.Message.Contains("duplicate"))
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Chỗ đễ xe đã được người khác đặt, vui lòng chọn chỗ mới",
//                        Success = false,
//                        StatusCode = 500
//                    };
//                }
//                else
//                {
//                    throw new Exception(ex.Message);
//                }
//            }
//       }
//    }
//}

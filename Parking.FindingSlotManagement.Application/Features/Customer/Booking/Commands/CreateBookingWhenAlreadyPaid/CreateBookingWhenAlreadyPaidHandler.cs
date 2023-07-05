//using AutoMapper;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons;
//using Parking.FindingSlotManagement.Application.Models.Booking;
//using Parking.FindingSlotManagement.Application.Models.PushNotification;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Parking.FindingSlotManagement.Domain.Enum;
//using QRCoder;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBookingWhenAlreadyPaid
//{
//    public class CreateBookingWhenAlreadyPaidHandler : IRequestHandler<CreateBookingWhenAlreadyPaidCommand, ServiceResponse<int>>
//    {
//        const int CUSTOMER = 3;
//        const int OTO = 1;
//        const int MOTO = 2;
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
//        private readonly ILogger<CreateBookingWhenAlreadyPaidHandler> _logger;
//        private readonly IParkingPriceRepository _parkingPriceRepository;
//        private readonly IConfiguration _configuration;
//        private readonly IFireBaseMessageServices _fireBaseMessageServices;
//        private readonly IVehicleInfoRepository _vehicleInfoRepository;
//        private readonly HttpClient _client;

//        public CreateBookingWhenAlreadyPaidHandler(IBookingRepository bookingRepository,
//            IParkingSlotRepository parkingSlotRepository,
//            ITrafficRepository trafficRepository,
//            IUserRepository userRepository,
//            IMapper mapper,
//            IFloorRepository floorRepository,
//            IParkingHasPriceRepository parkingHasPriceRepository,
//            IParkingRepository parkingRepository,
//            IVnPayRepository vnPayRepository,
//            ITimelineRepository timelineRepository,
//            ILogger<CreateBookingWhenAlreadyPaidHandler> logger,
//            IParkingPriceRepository parkingPriceRepository,
//            IConfiguration configuration,
//            IFireBaseMessageServices fireBaseMessageServices,
//            IVehicleInfoRepository vehicleInfoRepository)
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
//            _configuration = configuration;
//            _fireBaseMessageServices = fireBaseMessageServices;
//            _vehicleInfoRepository = vehicleInfoRepository;
//            _client = new HttpClient();
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "886d0b92410e625");
//        }
//        public async Task<ServiceResponse<int>> Handle(CreateBookingWhenAlreadyPaidCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var parkingSlot = await _parkingSlotRepository
//                    .GetById(request.BookingDto.ParkingSlotId);
//                if (parkingSlot == null)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Chỗ để xe không khả dụng",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                var vehicleInfor = await _vehicleInfoRepository
//                    .GetById(request.BookingDto.VehicleInforId);

//                if (vehicleInfor == null)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Loại xe không tồn tại",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                var user = await _userRepository
//                    .GetItemWithCondition(x => x.UserId == request.BookingDto.UserId &&
//                                            x.IsActive == true &&
//                                            x.RoleId == CUSTOMER);
//                if (user == null)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Người dùng không tồn tại",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }

//                var entity = _mapper.Map<Domain.Entities.Booking>(request.BookingDto);
//                entity.Status = BookingStatus.Payment_Successed.ToString();
//                entity.PaymentMethod = Domain.Enum.PaymentMethod.thanh_toan_online.ToString();
//                // get and set TMNCodeVnPay
//                var floor = await _floorRepository.GetById(parkingSlot.FloorId!);
//                var parkingId = floor.ParkingId;

//                var parking = await _parkingRepository.GetById(parkingId!);

//                //var vnpay = await _vnPayRepository
//                //    .GetItemWithCondition(x => x.ManagerId == managerId);
//                //entity.TmnCodeVnPay = vnpay.TmnCode;
//                /*List<Expression<Func<Domain.Entities.Booking, object>>> includes2 = new List<Expression<Func<Domain.Entities.Booking, object>>>
//                {
//                    x => x.ParkingSlot,
//                    x => x.ParkingSlot.Floor
//                };
//                var lstBooking = await _bookingRepository.GetAllItemWithCondition(x => x.ParkingSlotId == request.BookingDto.ParkingSlotId && x.ParkingSlot.FloorId == floor.FloorId && x.ParkingSlot.Floor.ParkingId == floor.ParkingId && x.DateBook.Date == DateTime.UtcNow.Date && x.Status != BookingStatus.Cancel.ToString(), includes2, null, true);
//                List<int> lstParkingSlotIdExist = new();
//                if(lstBooking.Count() > 0)
//                {
//                    foreach (var item in lstBooking)
//                    {
//                        if(request.BookingDto.StartTime < item.EndTime && request.BookingDto.EndTime > item.StartTime)
//                        {
//                            if(lstParkingSlotIdExist.Where(x => x.Equals(item.ParkingSlotId)).Count() <= 0)
//                            {
//                                lstParkingSlotIdExist.Add(item.ParkingSlotId);
//                            }
                            
//                        }
//                    }
//                }
//                List<Expression<Func<Domain.Entities.ParkingSlot, object>>> includes3 = new List<Expression<Func<Domain.Entities.ParkingSlot, object>>>
//                {
//                    x => x.Floor,
//                };
//                var lstParkingSlot = await _parkingSlotRepository.GetAllItemWithCondition(x => x.FloorId == floor.FloorId && x.Floor.ParkingId == floor.ParkingId, includes3, null, true);
//                var filterParkingSlot = lstParkingSlot.Where(item => !lstParkingSlotIdExist.Contains(item.ParkingSlotId)).ToList();*/

//                // Set TotalPrice

//                var trafficid = vehicleInfor.TrafficId;

//                if (parking.CarSpot <= 0 && trafficid == OTO)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Bãi không giữ xe ô tô, vui lòng chọn bãi khác",
//                        Success = true,
//                        StatusCode = 200,
//                    };
//                }
//                else if (parking.MotoSpot <= 0 && trafficid == MOTO)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Bãi không giữ xe máy, vui lòng chọn bãi khác",
//                        Success = true,
//                        StatusCode = 200,
//                    };
//                }

//                List<Expression<Func<ParkingHasPrice, object>>> includes = new List<Expression<Func<ParkingHasPrice, object>>>
//                {
//                    x => x.ParkingPrice!,
//                    x => x.ParkingPrice!.Traffic!
//                };

//                var parkingHasPrice = await _parkingHasPriceRepository
//                        .GetAllItemWithCondition(x => x.ParkingId == parkingId, includes);

//                var appliedParkingPriceId = parkingHasPrice
//                    .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficid)
//                    .FirstOrDefault()!.ParkingPriceId;

//                var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId);

//                var timeLines = await _timelineRepository
//                    .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);

//                decimal totalPrice = CaculatePriceBooking.CaculateExpectedPrice(request.BookingDto.StartTime, request.BookingDto.EndTime, parkingPrice, timeLines);

//                entity.TotalPrice = totalPrice;

//                var listBooking = await _bookingRepository
//                    .GetAllItemWithCondition(x => x.DateBook.Date <= DateTime.UtcNow.Date &&
//                    x.ParkingSlotId == request.BookingDto.ParkingSlotId);

//                DateTime Gioloinhat = new();

//                if (listBooking.Count() > 0)
//                {
//                    foreach (var item in listBooking)
//                    {
//                        if (Gioloinhat < item.EndTime)
//                        {
//                            Gioloinhat = item.EndTime.Value;
//                        }
//                    }
//                }

//                if (parkingSlot.IsAvailable == true)
//                {
//                    parkingSlot.IsAvailable = false;
//                    await _parkingSlotRepository.Save();
//                    entity.DateBook = DateTime.UtcNow.AddHours(7);

//                    await _bookingRepository.Insert(entity);

//                    var linkQRImage = await UploadQRImagess(entity.BookingId);
//                    var currentBooking = await _bookingRepository
//                        .GetItemWithCondition(x => x.BookingId == entity.BookingId, null!, false);

//                    currentBooking.QRImage = linkQRImage;

//                    await _bookingRepository.Save();

//                    var titleManager = _configuration.GetSection("MessageTitle_Manager").GetSection("Success").Value;
//                    var bodyManager = _configuration.GetSection("MessageBody_Manager").GetSection("Success").Value;

//                    var deviceToken = "";
//                    var managerAccount = await _userRepository.GetAllItemWithCondition(x => x.ParkingId == parking.ParkingId);
//                    var lstStaff = managerAccount.Where(x => x.RoleId == 2);
//                    var ManagerOfParking = managerAccount.FirstOrDefault(x => x.RoleId == 1);
//                    if (lstStaff.Any())
//                    {
//                        foreach (var item in lstStaff)
//                        {
//                            deviceToken = item.Devicetoken.ToString();
//                            var pushNotificationModel = new PushNotificationWebModel
//                            {
//                                Title = titleManager,
//                                Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                                TokenWeb = deviceToken,
//                            };
//                            await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
//                        }
//                    }
//                    else
//                    {
//                        var manager = await _userRepository.GetById(ManagerOfParking.UserId!);
//                        var pushNotificationModel = new PushNotificationWebModel
//                        {
//                            Title = titleManager,
//                            Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                            TokenWeb = manager.Devicetoken,
//                        };
//                        await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
//                    }

//                    var titleCustomer = _configuration.GetSection("MessageTitle_Customer").GetSection("Success").Value;
//                    var bodyCustomer = _configuration.GetSection("MessageBody_Customer").GetSection("Success").Value;

//                    var pushNotificationMobile = new PushNotificationMobileModel
//                    {
//                        Title = titleCustomer,
//                        Message = bodyCustomer + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                        TokenMobile = request.DeviceToKenMobile,
//                    };

//                    await _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);

//                    return new ServiceResponse<int>
//                    {
//                        Data = entity.BookingId,
//                        StatusCode = 201,
//                        Success = true,
//                    };
//                }
//                else if (parkingSlot.IsAvailable == false && request.BookingDto.StartTime >= Gioloinhat)
//                {

//                    entity.DateBook = DateTime.UtcNow.AddHours(7);

//                    await _bookingRepository.Insert(entity);

//                    var linkQRImage = await UploadQRImagess(entity.BookingId);
//                    var currentBooking = await _bookingRepository
//                        .GetItemWithCondition(x => x.BookingId == entity.BookingId, null!, false);

//                    currentBooking.QRImage = linkQRImage;

//                    await _bookingRepository.Save();

//                    var titleManager = _configuration.GetSection("MessageTitle_Manager").GetSection("Success").Value;
//                    var bodyManager = _configuration.GetSection("MessageBody_Manager").GetSection("Success").Value;

//                    var deviceToken = "";
//                    var managerAccount = await _userRepository.GetAllItemWithCondition(x => x.ParkingId == parking.ParkingId);
//                    var lstStaff = managerAccount.Where(x => x.RoleId == 2);
//                    var ManagerOfParking = managerAccount.FirstOrDefault(x => x.RoleId == 1);
//                    if (lstStaff.Any())
//                    {
//                        foreach (var item in lstStaff)
//                        {
//                            deviceToken = item.Devicetoken.ToString();
//                            var pushNotificationModel = new PushNotificationWebModel
//                            {
//                                Title = titleManager,
//                                Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                                TokenWeb = deviceToken,
//                            };
//                            await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
//                        }
//                    }
//                    else
//                    {
//                        var manager = await _userRepository.GetById(ManagerOfParking.UserId!);
//                        var pushNotificationModel = new PushNotificationWebModel
//                        {
//                            Title = titleManager,
//                            Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                            TokenWeb = manager.Devicetoken,
//                        };
//                        await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
//                    }

//                    var titleCustomer = _configuration.GetSection("MessageTitle_Customer").GetSection("Success").Value;
//                    var bodyCustomer = _configuration.GetSection("MessageBody_Customer").GetSection("Success").Value;

//                    var pushNotificationMobile = new PushNotificationMobileModel
//                    {
//                        Title = titleCustomer,
//                        Message = bodyCustomer + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
//                        TokenMobile = request.DeviceToKenMobile,
//                    };

//                    await _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);

//                    return new ServiceResponse<int>
//                    {
//                        Data = entity.BookingId,
//                        StatusCode = 201,
//                        Success = true,
//                    };
//                }

//                return new ServiceResponse<int>
//                {
//                    Message = "Chỗ đỗ xe đã được người khác đặt, vui lòng chọn chỗ mới",
//                    Success = false,
//                    StatusCode = 400
//                };
//            }
//            catch (DbUpdateException ex)
//            {
//                if (ex.InnerException!.Message.Contains("duplicate"))
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Chỗ đỗ xe đã được người khác đặt, vui lòng chọn chỗ mới",
//                        Success = false,
//                        StatusCode = 400
//                    };
//                }
//                else
//                {
//                    throw new Exception(ex.Message);
//                }
//            }
//        }
//        private static decimal CaculateTotalPrice(CreateBookingWhenAlreadyPaidCommand request, ParkingPrice parkingPrice, IEnumerable<TimeLine> timeLines)
//        {
//            var startTimeBooking = request.BookingDto.StartTime.TimeOfDay.TotalHours;
//            var endTimeBooking = request.BookingDto.EndTime.TimeOfDay.TotalHours;
//            var startTimeDate = request.BookingDto.StartTime.Date;
//            var endTimeDate = request.BookingDto.EndTime.Date;
//            var startingTime = parkingPrice.StartingTime;
//            var extraTimeStep = parkingPrice.ExtraTimeStep;
//            bool foundStartPoint = false;
//            var hitEndPoint = false;
//            var totalPrice = 0M;
//            var startingPoint = 0;
//            var extraFeePoint = 0;
//            var isPass = false;

//            foreach (var package in timeLines)
//            {
//                var startTimePackage = package.StartTime?.TotalHours;
//                var endTimePackage = package.EndTime?.TotalHours;
//                if (startTimeDate == endTimeDate)
//                {
//                    // một gói, trong ngày
//                    if (startTimePackage > endTimePackage)
//                    {
//                        endTimePackage += 24;
//                    }
//                    //if (startTimeBooking < startTimePackage)
//                    //{
//                    //    startTimeBooking += 24;
//                    //    endTimeBooking += 24;
//                    //}
//                    if (startTimeBooking >= startTimePackage &&
//                        startTimeBooking <= endTimePackage &&
//                        endTimeBooking > startTimeBooking &&
//                        endTimeBooking >= startTimePackage &&
//                        endTimeBooking <= endTimePackage ||
//                        (startTimeBooking + 24) >= startTimePackage &&
//                        (startTimeBooking + 24) <= endTimePackage &&
//                        (endTimeBooking + 24) > (startTimeBooking + 24) &&
//                        (endTimeBooking + 24) >= startTimePackage &&
//                        (endTimeBooking + 24) <= endTimePackage)
//                    {
//                        if (package.StartTime > package.EndTime)
//                        {
//                            if (Math.Abs(24 - startTimeBooking) >= 24 - startTimePackage &&
//                                Math.Abs(24 - endTimeBooking) >= 24 - startTimePackage)
//                            {
//                                startTimeBooking += 24;
//                                endTimeBooking += 24;
//                            }
//                            package.EndTime += TimeSpan.FromHours(24);
//                        }

//                        if (startTimeBooking >= startTimePackage &&
//                            startTimeBooking <= endTimePackage &&
//                            endTimeBooking >= startTimePackage &&
//                            endTimeBooking <= endTimePackage)
//                        {
//                            var so_tieng_book = (decimal)(endTimeBooking - startTimeBooking);
//                            if (startingTime < so_tieng_book)
//                            {
//                                var so_tieng_tinh_ExtraFee = so_tieng_book - startingTime;
//                                var so_step = (int)so_tieng_tinh_ExtraFee / (int)extraTimeStep!;
//                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee) + (decimal)package.ExtraFee;
//                            }
//                            else
//                            {
//                                totalPrice = package.Price;
//                            }
//                        }

//                        if (endTimeBooking > 24 &&
//                            startTimeBooking > 24)
//                        {
//                            endTimeBooking -= 24;
//                            startTimeBooking -= 24;
//                        }
//                        break;
//                    }

//                    // nhiều gói, trong ngày
//                    else
//                    {
//                        while (hitEndPoint == false)
//                        {
//                            if (endTimePackage > 24 && startTimeBooking < 24)
//                            {
//                                startTimeBooking += 24;
//                            }
//                            if (startTimeBooking >= startTimePackage &&
//                                startTimeBooking < endTimePackage &&
//                                foundStartPoint == false)
//                            {
//                                foundStartPoint = true;
//                                var step = 0;
//                                var startingPrice = 0M;

//                                startingPrice =
//                                    (double)(startTimeBooking + startingTime)! == endTimePackage
//                                    ? package.Price
//                                    : package.Price + (decimal)package.ExtraFee!;

//                                var extraPrice = 0M;
//                                startingPoint = (int)(startTimeBooking + startingTime)!;
//                                if (startingPoint == endTimePackage)
//                                {
//                                    isPass = true;
//                                }
//                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;
//                                while (extraFeePoint <= endTimePackage)
//                                {
//                                    step++;
//                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
//                                    extraPrice = step * (decimal)package.ExtraFee!;
//                                };
//                                totalPrice += startingPrice + extraPrice;

//                                break;
//                            }

//                            if (foundStartPoint == true)
//                            {
//                                var priceOfTimeLineTwo = 0M;
//                                if (startingPoint == startTimePackage ||
//                                    startingPoint == startTimePackage + 24)
//                                {
//                                    totalPrice += (decimal)package.ExtraFee!;
//                                }
//                                if (extraFeePoint > 24 && endTimeBooking < 24)
//                                {
//                                    endTimeBooking += 24;
//                                }
//                                while (extraFeePoint <= endTimeBooking)
//                                {
//                                    priceOfTimeLineTwo = (decimal)package.ExtraFee!;
//                                    totalPrice += priceOfTimeLineTwo;
//                                    extraFeePoint += (int)extraTimeStep!;
//                                    if (endTimePackage < startTimePackage)
//                                    {
//                                        endTimePackage += 24;
//                                    }
//                                    if (extraFeePoint > 24 && endTimePackage < 24)
//                                    {
//                                        endTimePackage += 24;
//                                    }
//                                    if (extraFeePoint > endTimePackage)
//                                    {
//                                        if (extraFeePoint > 24)
//                                        {
//                                            extraFeePoint -= 24;
//                                            endTimeBooking -= 24;
//                                            package.EndTime -= TimeSpan.FromHours(24);
//                                        }
//                                        break;
//                                    }
//                                }
//                                if (extraFeePoint > endTimeBooking)
//                                {
//                                    isPass = false;
//                                    break;
//                                }
//                                else break;
//                            }
//                            if (startTimeBooking > 24)
//                            {
//                                startTimeBooking -= 24;
//                            }

//                            if (extraFeePoint > endTimeBooking && isPass == false)
//                            {
//                                hitEndPoint = true;
//                                break;
//                            }
//                            break;
//                        }
//                    }
//                }

//                if (startTimeDate < endTimeDate)
//                {
//                    if (endTimeBooking == 0)
//                    {
//                        endTimeBooking += 24;
//                    }
//                    if (startTimePackage > endTimePackage)
//                    {
//                        endTimePackage += 24;
//                    }
//                    // một gói, qua ngày
//                    if (startTimeBooking >= startTimePackage &&
//                        startTimeBooking < endTimePackage &&
//                        endTimeBooking > startTimeBooking &&
//                        endTimeBooking >= startTimePackage &&
//                        endTimeBooking <= endTimePackage)
//                    {
//                        if (package.StartTime > package.EndTime)
//                        {
//                            if (startTimeBooking > endTimeBooking)
//                            {
//                                endTimeBooking += 24;
//                            }
//                            package.EndTime += TimeSpan.FromHours(24);
//                        }

//                        if (startTimeBooking >= startTimePackage &&
//                            startTimeBooking < endTimePackage &&
//                            endTimeBooking >= startTimePackage &&
//                            endTimeBooking <= endTimePackage)
//                        {
//                            var so_tieng_book = (decimal)(endTimeBooking - startTimeBooking);
//                            if (startingTime < so_tieng_book)
//                            {
//                                var so_tieng_tinh_ExtraFee = so_tieng_book - startingTime;
//                                var so_step = (int)so_tieng_tinh_ExtraFee / (int)extraTimeStep!;
//                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee!) + (decimal)package.ExtraFee!;
//                            }
//                            else
//                            {
//                                totalPrice = package.Price;
//                            }
//                        }

//                        if (endTimeBooking > 24)
//                        {
//                            endTimeBooking -= 24;
//                        }
//                        break;
//                    }

//                    // nhiều gói, qua ngày
//                    else
//                    {
//                        while (hitEndPoint == false)
//                        {
//                            if (endTimePackage < startTimePackage)
//                            {
//                                package.EndTime += TimeSpan.FromHours(24);
//                            }

//                            if (startTimeBooking >= startTimePackage &&
//                                startTimeBooking < endTimePackage &&
//                                foundStartPoint == false)
//                            {
//                                foundStartPoint = true;
//                                var step = 0;
//                                var startingPrice = 0M;

//                                startingPrice =
//                                    (double)(startTimeBooking + startingTime)! == endTimePackage
//                                    ? package.Price
//                                    : package.Price + (decimal)package.ExtraFee!;

//                                var extraPrice = 0M;
//                                var bookedTime = (int)(endTimePackage - startTimeBooking);
//                                startingPoint = (int)(startTimeBooking + startingTime!);

//                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;
//                                while (extraFeePoint <= endTimePackage)
//                                {
//                                    step++;
//                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
//                                    extraPrice = step * (decimal)package.ExtraFee!;
//                                };
//                                totalPrice = startingPrice + extraPrice;

//                                break;
//                            }

//                            if (foundStartPoint)
//                            {
//                                if (startingPoint == startTimePackage)
//                                {
//                                    totalPrice += (decimal)package.ExtraFee!;
//                                }
//                                while (extraFeePoint <= endTimeBooking)
//                                {
//                                    totalPrice += (decimal)package.ExtraFee!;
//                                    extraFeePoint += (int)extraTimeStep!;
//                                    if (endTimePackage < startTimePackage)
//                                    {
//                                        endTimePackage += 24;
//                                    }
//                                    if (extraFeePoint > 24 && endTimePackage < 24)
//                                    {
//                                        endTimePackage += 24;
//                                    }
//                                    if (extraFeePoint > endTimePackage)
//                                    {
//                                        if (extraFeePoint > 24)
//                                        {
//                                            extraFeePoint -= 24;
//                                            endTimeBooking -= 24;
//                                            package.EndTime -= TimeSpan.FromHours(24);
//                                        }
//                                        break;
//                                    }
//                                }
//                            }
//                            if (extraFeePoint >= endTimeBooking)
//                            {
//                                hitEndPoint = true;
//                                break;
//                            }
//                            break;
//                        }
//                    }
//                }
//            }

//            return totalPrice;
//        }


//        private async Task<string> UploadQRImagess(int bookingId)
//        {
//            QRCodeGenerator qrGenerator = new QRCodeGenerator();

//            // Generate a QR code with the given data
//            QRCodeData qrCodeData = qrGenerator.CreateQrCode(bookingId.ToString(), QRCodeGenerator.ECCLevel.Q);

//            // Create a QR code object from the QR code data
//            QRCode qrCode = new QRCode(qrCodeData);

//            // Convert the QR code to a bitmap image
//            Bitmap qrCodeImage = qrCode.GetGraphic(20);
//            IFormFile file = ConvertToIFormFile(qrCodeImage, "qrCodeImage.jpg");
//            var ms = new MemoryStream();
//            await file.CopyToAsync(ms);
//            var content = new ByteArrayContent(ms.ToArray());
//            /*content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);*/
//            var response = await _client.PostAsync("https://api.imgur.com/3/image", content);
//            response.EnsureSuccessStatusCode();
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurResponse>(responseContent);
//            return result.Data.Link;
//        }

//        private IFormFile ConvertToIFormFile(Bitmap bitmap, string fileName)
//        {
//            var stream = new MemoryStream();

//            // Save the bitmap to the stream using the desired image format
//            bitmap.Save(stream, ImageFormat.Jpeg);

//            // Reset the stream position to the beginning
//            stream.Position = 0;

//            // Create an IFormFile from the stream
//            var formFile = new FormFile(stream, 0, stream.Length, "qrCodeImage", fileName);

//            return formFile;
//        }
//    }
//}

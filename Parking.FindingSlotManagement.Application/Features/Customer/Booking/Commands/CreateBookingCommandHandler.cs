using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ServiceResponse<string>>
    {
        const int CUSTOMER = 3;
        const int OTO = 1;
        const int MOTO = 2;
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly ITrafficRepository _trafficRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IVnPayRepository _vnPayRepository;
        private readonly ITimelineRepository _timelineRepository;
        private readonly ILogger<CreateBookingCommandHandler> _logger;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly HttpClient _client;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
            IParkingSlotRepository parkingSlotRepository,
            ITrafficRepository trafficRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IFloorRepository floorRepository,
            IParkingHasPriceRepository parkingHasPriceRepository,
            IParkingRepository parkingRepository,
            IVnPayRepository vnPayRepository,
            ITimelineRepository timelineRepository,
            ILogger<CreateBookingCommandHandler> logger,
            IParkingPriceRepository parkingPriceRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingSlotRepository = parkingSlotRepository;
            _trafficRepository = trafficRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _floorRepository = floorRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingRepository = parkingRepository;
            _vnPayRepository = vnPayRepository;
            _timelineRepository = timelineRepository;
            _logger = logger;
            _parkingPriceRepository = parkingPriceRepository;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "886d0b92410e625");
        }

        public async Task<ServiceResponse<string>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingSlot = await _parkingSlotRepository.GetById(request.ParkingSlotId);
                if (parkingSlot == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ để xe không khả dụng",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var vehicleInfor = await _trafficRepository.GetById(request.VehicleInforId);
                if (vehicleInfor == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Loại xe không tồn tại",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var user = await _userRepository
                    .GetItemWithCondition(x => x.UserId == request.UserId &&
                                            x.IsActive == true &&
                                            x.RoleId == CUSTOMER);
                if (user == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Người dùng không tồn tại",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var entity = _mapper.Map<Domain.Entities.Booking>(request);
                entity.Status = BookingStatus.Initial.ToString();
                // get and set TMNCodeVnPay
                var floor = await _floorRepository.GetById(parkingSlot.FloorId!);
                var parkingId = floor.ParkingId;

                var parking = await _parkingRepository.GetById(parkingId!);
                var managerId = parking.ManagerId;

                //var vnpay = await _vnPayRepository
                //    .GetItemWithCondition(x => x.ManagerId == managerId);
                //entity.TmnCodeVnPay = vnpay.TmnCode;

                // Set TotalPrice

                var trafficid = vehicleInfor.TrafficId;

                if (parking.CarSpot <= 0 && trafficid == OTO)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi không giữ xe ô tô, vui lòng chọn bãi khác",
                        Success = true,
                        StatusCode = 200,
                    };
                }
                else if (parking.MotoSpot <= 0 && trafficid == MOTO)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi không giữ xe máy, vui lòng chọn bãi khác",
                        Success = true,
                        StatusCode = 200,
                    };
                }

                List<Expression<Func<ParkingHasPrice, object>>> includes = new List<Expression<Func<ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice!,
                    x => x.ParkingPrice!.Traffic!
                };

                var parkingHasPrice = await _parkingHasPriceRepository
                        .GetAllItemWithCondition(x => x.ParkingId == parkingId, includes);

                var appliedParkingPriceId = parkingHasPrice
                    .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficid)
                    .FirstOrDefault()!.ParkingPriceId;

                var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId);

                var timeLines = await _timelineRepository
                    .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);

                var startTimeBooking = request.StartTime.TimeOfDay.TotalHours;
                var endTimeBooking = request.EndTime.TimeOfDay.TotalHours;
                var startTimeDate = request.StartTime.Date;
                var endTimeDate = request.EndTime.Date;
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
                        if (startTimeBooking >= startTimePackage &&
                            startTimeBooking <= endTimePackage &&
                            endTimeBooking > startTimeBooking &&
                            endTimeBooking >= startTimePackage &&
                            endTimeBooking <= endTimePackage)
                        {
                            if (package.StartTime > package.EndTime)
                            {
                                if (Math.Abs(24 - startTimeBooking) >= (24 - startTimePackage) &&
                                    Math.Abs(24 - endTimeBooking) >= (24 - startTimePackage))
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
                                    startingPoint = (int)(startTimeBooking + startingTime)!; // 6
                                    if (startingPoint == endTimePackage)
                                    {
                                        isPass = true;
                                    }
                                    extraFeePoint = (int)(startingPoint + extraTimeStep)!; // 8
                                    while (extraFeePoint <= endTimePackage)
                                    {
                                        step++;
                                        extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
                                        extraPrice = (step * (decimal)package.ExtraFee!);
                                    };
                                    totalPrice += startingPrice + extraPrice;

                                    break;
                                }

                                if (foundStartPoint == true)
                                {
                                    var priceOfTimeLineTwo = 0M;
                                    if (startingPoint == startTimePackage ||
                                        startingPoint == (startTimePackage + 24))
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

                        }
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
                                        extraPrice = (step * (decimal)package.ExtraFee!);
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

                entity.TotalPrice = totalPrice;

                await _bookingRepository.Insert(entity);

                var linkQRImage = await UploadQRImagess(entity.BookingId);
                var currentBooking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == entity.BookingId, null!, false);

                currentBooking.QRImage = linkQRImage;

                await _bookingRepository.Save();

                return new ServiceResponse<string>
                {
                    Data = totalPrice.ToString(),
                    StatusCode = 201,
                    Success = true,
                };

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đỗ xe đã được người khác đặt, vui lòng chọn chỗ mới",
                        Success = false,
                        StatusCode = 500
                    };
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private async Task<string> UploadQRImagess(int bookingId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            // Generate a QR code with the given data
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(bookingId.ToString(), QRCodeGenerator.ECCLevel.Q);

            // Create a QR code object from the QR code data
            QRCode qrCode = new QRCode(qrCodeData);

            // Convert the QR code to a bitmap image
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            IFormFile file = ConvertToIFormFile(qrCodeImage, "qrCodeImage.jpg");
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var content = new ByteArrayContent(ms.ToArray());
            /*content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);*/
            var response = await _client.PostAsync("https://api.imgur.com/3/image", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurResponse>(responseContent);
            return result.Data.Link;
        }

        private IFormFile ConvertToIFormFile(Bitmap bitmap, string fileName)
        {
            var stream = new MemoryStream();

            // Save the bitmap to the stream using the desired image format
            bitmap.Save(stream, ImageFormat.Jpeg);

            // Reset the stream position to the beginning
            stream.Position = 0;

            // Create an IFormFile from the stream
            var formFile = new FormFile(stream, 0, stream.Length, "qrCodeImage", fileName);

            return formFile;
        }
    }
}

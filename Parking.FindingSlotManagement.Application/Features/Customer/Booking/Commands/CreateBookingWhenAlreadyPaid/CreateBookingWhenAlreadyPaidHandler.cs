using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBookingWhenAlreadyPaid
{
    public class CreateBookingWhenAlreadyPaidHandler : IRequestHandler<CreateBookingWhenAlreadyPaidCommand, ServiceResponse<int>>
    {
        const int CUSTOMER = 3;
        const int MANAGER = 1;
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
        private readonly ILogger<CreateBookingWhenAlreadyPaidHandler> _logger;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IConfiguration _configuration;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly HttpClient _client;

        public CreateBookingWhenAlreadyPaidHandler(IBookingRepository bookingRepository,
            IParkingSlotRepository parkingSlotRepository,
            ITrafficRepository trafficRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IFloorRepository floorRepository,
            IParkingHasPriceRepository parkingHasPriceRepository,
            IParkingRepository parkingRepository,
            IVnPayRepository vnPayRepository,
            ITimelineRepository timelineRepository,
            ILogger<CreateBookingWhenAlreadyPaidHandler> logger,
            IParkingPriceRepository parkingPriceRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices,
            IVehicleInfoRepository vehicleInfoRepository,
            ITimeSlotRepository timeSlotRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository)
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
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
            _vehicleInfoRepository = vehicleInfoRepository;
            _timeSlotRepository = timeSlotRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "886d0b92410e625");
        }
        public async Task<ServiceResponse<int>> Handle(CreateBookingWhenAlreadyPaidCommand request, CancellationToken cancellationToken)
        {

            var startTimeBooking = request.BookingDto.StartTime;
            var endTimeBooking = request.BookingDto.EndTime;
            var parkingSlotId = request.BookingDto.ParkingSlotId;
            var paymentMethod = request.BookingDto.PaymentMethod;
            try
            {

                var includes = new List<Expression<Func<Domain.Entities.TimeSlot, object>>>
                {
                   x => x.Parkingslot,
                   x => x.Parkingslot.Floor
                };
                var currentLstBookedSlot = await _timeSlotRepository
                    .GetAllItemWithCondition(x => x.ParkingSlotId == request.BookingDto.ParkingSlotId &&
                                                  x.StartTime >= startTimeBooking &&
                                                  x.EndTime <= endTimeBooking &&
                                                  x.Status == "Booked", includes);

                if (currentLstBookedSlot.Any())
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Chỗ đặt đã được đặt, vui lòng chọn chỗ khác.",
                        StatusCode = 400,
                        Success = true,
                    };
                }

                var parkingSlot = await _parkingSlotRepository
                    .GetById(parkingSlotId);

                if (parkingSlot == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Chỗ để xe không khả dụng",
                        Success = true,
                        StatusCode = 200
                    };
                }

                var vehicleInfor = await _vehicleInfoRepository
                    .GetById(request.BookingDto.VehicleInforId);

                if (vehicleInfor == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Loại xe không tồn tại",
                        Success = true,
                        StatusCode = 200
                    };
                }

                List<Expression<Func<User, object>>> includesWallet = new()
                {
                    x => x.Wallet,
                };

                var user = await _userRepository
                    .GetItemWithCondition(x => x.UserId == request.BookingDto.UserId &&
                                            x.IsActive == true &&
                                            x.RoleId == CUSTOMER, includesWallet, false);

                if (user == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Người dùng không tồn tại",
                        Success = true,
                        StatusCode = 200
                    };
                }

                var entity = _mapper.Map<Domain.Entities.Booking>(request.BookingDto);
                entity.Status = BookingStatus.Initial.ToString();
                var floor = await _floorRepository.GetById(parkingSlot.FloorId!);
                var parkingId = floor.ParkingId;
                var parking = await _parkingRepository.GetById(parkingId!);
                var trafficid = vehicleInfor.TrafficId;

                if (parking.CarSpot <= 0 && trafficid == OTO)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi không giữ xe ô tô, vui lòng chọn bãi khác",
                        Success = true,
                        StatusCode = 200,
                    };
                }
                else if (parking.MotoSpot <= 0 && trafficid == MOTO)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi không giữ xe máy, vui lòng chọn bãi khác",
                        Success = true,
                        StatusCode = 200,
                    };
                }

                decimal expectedPrice = await CaculateExpectedPrice(request, parkingId, trafficid);

                if (user.Wallet.Balance < expectedPrice)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Số dư ví không đủ, vui lòng nạp thêm hoặc chọn phương thức thanh toán khác.",
                    };
                }
                else
                {
                    var managerWallet = await _userRepository
                        .GetItemWithCondition(x => x.ParkingId == parkingId && x.RoleId == MANAGER, includesWallet, false);

                    managerWallet.Wallet!.Balance += expectedPrice;
                    user.Wallet.Balance -= expectedPrice;
                    await _walletRepository.Save();
                }

                entity.TotalPrice = expectedPrice;
                entity.DateBook = DateTime.Now;
                await _bookingRepository.Insert(entity);

                var linkQRImage = await UploadQRImagess(entity.BookingId);
                var currentBooking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == entity.BookingId, null!, false);
                currentBooking.QRImage = linkQRImage;
                await _bookingRepository.Save();

                var timeSlotsBooking = await _timeSlotRepository
                    .GetAllTimeSlotsBooking(startTimeBooking, endTimeBooking, parkingSlotId);
                var bookingDetails = new List<BookingDetails>();
                foreach (var timeSlot in timeSlotsBooking)
                {
                    bookingDetails.Add(new BookingDetails { BookingId = entity.BookingId, TimeSlotId = timeSlot.TimeSlotId });
                    timeSlot.Status = TimeSlotStatus.Booked.ToString();
                }

                await _timeSlotRepository.Save();
                await _bookingDetailsRepository.AddRange(bookingDetails);

                await CreateNewTransaction(paymentMethod, user, entity, expectedPrice);
                await PushNotiToManager(parkingSlot, floor, parking);
                await PushNoTiToCustomer(request, parkingSlot, floor);

                return new ServiceResponse<int>
                {
                    Data = entity.BookingId,
                    StatusCode = 201,
                    Success = true,
                };
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Chỗ đỗ xe đã được người khác đặt, vui lòng chọn chỗ mới",
                        Success = false,
                        StatusCode = 400
                    };
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private async Task CreateNewTransaction(string? paymentMethod, User user, Domain.Entities.Booking entity, decimal expectedPrice)
        {
            var transaction = new Domain.Entities.Transaction
            {
                Price = expectedPrice,
                Status = BookingPaymentStatus.Da_thanh_toan.ToString(),
                PaymentMethod = paymentMethod,
                WalletId = user.Wallet.WalletId,
                BookingId = entity.BookingId
            };
            await _transactionRepository.Insert(transaction);
        }

        private async Task PushNoTiToCustomer(CreateBookingWhenAlreadyPaidCommand request, Domain.Entities.ParkingSlot parkingSlot, Floor floor)
        {
            var titleCustomer = _configuration.GetSection("MessageTitle_Customer").GetSection("Success").Value;
            var bodyCustomer = _configuration.GetSection("MessageBody_Customer").GetSection("Success").Value;

            var pushNotificationMobile = new PushNotificationMobileModel
            {
                Title = titleCustomer,
                Message = bodyCustomer + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
                TokenMobile = request.DeviceToKenMobile,
            };

            await _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);
        }

        private async Task PushNotiToManager(Domain.Entities.ParkingSlot parkingSlot, Floor floor, Domain.Entities.Parking parking)
        {
            var titleManager = _configuration.GetSection("MessageTitle_Manager").GetSection("Success").Value;
            var bodyManager = _configuration.GetSection("MessageBody_Manager").GetSection("Success").Value;

            var deviceToken = "";
            var managerAccount = await _userRepository.GetAllItemWithCondition(x => x.ParkingId == parking.ParkingId);
            var lstStaff = managerAccount.Where(x => x.RoleId == 2);
            var ManagerOfParking = managerAccount.FirstOrDefault(x => x.RoleId == 1);

            if (lstStaff.Any())
            {
                foreach (var item in lstStaff)
                {
                    deviceToken = item.Devicetoken.ToString();
                    var pushNotificationModel = new PushNotificationWebModel
                    {
                        Title = titleManager,
                        Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
                        TokenWeb = deviceToken,
                    };
                    await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                }
            }
            else
            {
                var manager = await _userRepository.GetById(ManagerOfParking.UserId!);
                var pushNotificationModel = new PushNotificationWebModel
                {
                    Title = titleManager,
                    Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
                    TokenWeb = manager.Devicetoken,
                };
                await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
            }
        }

        private async Task<decimal> CaculateExpectedPrice(CreateBookingWhenAlreadyPaidCommand request, int? parkingId, int? trafficid)
        {
            List<Expression<Func<ParkingHasPrice, object>>> includess = new List<Expression<Func<ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice!,
                    x => x.ParkingPrice!.Traffic!
                };
            var parkingHasPrice = await _parkingHasPriceRepository
                    .GetAllItemWithCondition(x => x.ParkingId == parkingId, includess);
            var appliedParkingPriceId = parkingHasPrice
                .Where(x => x.ParkingPrice!.Traffic!.TrafficId == trafficid)
                .FirstOrDefault()!.ParkingPriceId;
            var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId);
            var timeLines = await _timelineRepository
                .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);
            decimal expectedPrice = CaculatePriceBooking
                .CaculateExpectedPrice(request.BookingDto.StartTime, request.BookingDto.EndTime,
                parkingPrice, timeLines);
            return expectedPrice;
        }

        private async Task<string> UploadQRImagess(int bookingId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            // Generate a QR code with the given data
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("pz-"+bookingId.ToString(), QRCodeGenerator.ECCLevel.Q);

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

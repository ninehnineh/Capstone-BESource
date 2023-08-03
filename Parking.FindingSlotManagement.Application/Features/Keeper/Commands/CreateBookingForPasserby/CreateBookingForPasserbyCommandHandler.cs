using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.CreateBookingForPasserby
{
    public class CreateBookingForPasserbyCommandHandler : IRequestHandler<CreateBookingForPasserbyCommand, ServiceResponse<int>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly HttpClient _client;

        public CreateBookingForPasserbyCommandHandler(IBookingRepository bookingRepository, 
            IMapper mapper, 
            ITimeSlotRepository timeSlotRepository, 
            IParkingSlotRepository parkingSlotRepository,
            IVehicleInfoRepository vehicleInfoRepository,
            IFloorRepository floorRepository,
            IParkingHasPriceRepository parkingHasPriceRepository,
            IParkingPriceRepository parkingPriceRepository,
            ITimelineRepository timelineRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            ITransactionRepository transactionRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _timeSlotRepository = timeSlotRepository;
            _parkingSlotRepository = parkingSlotRepository;
            _vehicleInfoRepository = vehicleInfoRepository;
            _floorRepository = floorRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
            _parkingPriceRepository = parkingPriceRepository;
            _timelineRepository = timelineRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _transactionRepository = transactionRepository;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "886d0b92410e625");
        }
        public async Task<ServiceResponse<int>> Handle(CreateBookingForPasserbyCommand request, CancellationToken cancellationToken)
        {
            var startTimeBooking = DateTime.UtcNow.AddHours(7);
            var endTimeBooking = request.BookingForPasserby.EndTime;
            var parkingSlotId = request.BookingForPasserby.ParkingSlotId;
            try
            {
                var includes = new List<Expression<Func<Domain.Entities.TimeSlot, object>>>
                {
                   x => x.Parkingslot,
                   x => x.Parkingslot.Floor
                };
                var currentLstBookedSlot = await _timeSlotRepository.GetAllItemWithCondition(x =>
                                                            x.ParkingSlotId == request.BookingForPasserby.ParkingSlotId &&
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
                var vehicleInforEntity = _mapper.Map<VehicleInfor>(request.VehicleInformationForPasserby);
                vehicleInforEntity.TrafficId = 1;
                await _vehicleInfoRepository.Insert(vehicleInforEntity);
                Domain.Entities.Booking bookingEntity = new()
                {
                    StartTime = startTimeBooking.Date.AddHours(startTimeBooking.Hour),
                    CheckinTime = startTimeBooking,
                    EndTime = request.BookingForPasserby.EndTime,
                    GuestName = request.BookingForPasserby.GuestName,
                    GuestPhone = request.BookingForPasserby.GuestPhone,
                    VehicleInforId = vehicleInforEntity.VehicleInforId,
                    Status = BookingStatus.Check_In.ToString(),
                };
                var floor = await _floorRepository.GetById(parkingSlot.FloorId!);
                var parkingId = floor.ParkingId;
                List<Expression<Func<ParkingHasPrice, object>>> includess = new List<Expression<Func<ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice!,
                    x => x.ParkingPrice!.Traffic!
                };

                var parkingHasPrice = await _parkingHasPriceRepository
                        .GetAllItemWithCondition(x => x.ParkingId == parkingId, includess);

                var appliedParkingPriceId = parkingHasPrice
                    .Where(x => x.ParkingPrice!.Traffic!.TrafficId == 1)
                    .FirstOrDefault()!.ParkingPriceId;

                var parkingPrice = await _parkingPriceRepository.GetById(appliedParkingPriceId);

                var timeLines = await _timelineRepository
                    .GetAllItemWithCondition(x => x.ParkingPriceId == appliedParkingPriceId);
                decimal expectedPrice = CaculatePriceBooking
                    .CaculateExpectedPrice(startTimeBooking.Date.AddHours(startTimeBooking.Hour), request.BookingForPasserby.EndTime,
                    parkingPrice, timeLines);
                bookingEntity.TotalPrice = expectedPrice;
                bookingEntity.DateBook = DateTime.UtcNow.AddHours(7);

                await _bookingRepository.Insert(bookingEntity);
                var linkQRImage = await UploadQRImagess(bookingEntity.BookingId);
                var currentBooking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == bookingEntity.BookingId, null!, false);
                currentBooking.QRImage = linkQRImage;

                await _bookingRepository.Save();
                var timeSlotsBooking = await _timeSlotRepository
                    .GetAllTimeSlotsBooking(startTimeBooking.Date.AddHours(startTimeBooking.Hour), endTimeBooking, parkingSlotId);

                var bookingDetails = new List<BookingDetails>();

                foreach (var timeSlot in timeSlotsBooking)
                {
                    bookingDetails.Add(new BookingDetails { BookingId = bookingEntity.BookingId, TimeSlotId = timeSlot.TimeSlotId });
                    timeSlot.Status = TimeSlotStatus.Booked.ToString();
                }

                await _timeSlotRepository.Save();
                await _bookingDetailsRepository.AddRange(bookingDetails);
                var transaction = new Transaction
                {
                    Price = expectedPrice,
                    Status = BookingPaymentStatus.Chua_thanh_toan.ToString(),
                    PaymentMethod = PaymentMethod.tra_sau.ToString(),
                    BookingId = bookingEntity.BookingId
                };
                await _transactionRepository.Insert(transaction);
                return new ServiceResponse<int>
                {
                    Data = bookingEntity.BookingId,
                    StatusCode = 201,
                    Success = true,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private async Task<string> UploadQRImagess(int bookingId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            // Generate a QR code with the given data
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("pz-" + bookingId.ToString(), QRCodeGenerator.ECCLevel.Q);

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

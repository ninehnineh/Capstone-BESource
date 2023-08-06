using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetAllBookingByParkingIdQueryHandlerTests
    {
        private readonly GetAllBookingByParkingIdQueryHandler _handler;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        public GetAllBookingByParkingIdQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllBookingByParkingIdQueryHandler(_parkingRepositoryMock.Object, _bookingRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_NonExistentParking_ShouldReturnError()
        {
            // Arrange
            var parkingId = 1;
            var request = new GetAllBookingByParkingIdQuery { ParkingId = parkingId };

            _parkingRepositoryMock.Setup(repo => repo.GetById(parkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            result.StatusCode.ShouldBe(404);
        }
        [Fact]
        public async Task Handle_NoBookingsFound_ShouldReturnError()
        {
            // Arrange
            var parkingId = 1;
            var request = new GetAllBookingByParkingIdQuery { ParkingId = parkingId, PageNo = 1, PageSize = 10 };

            var parking = new Domain.Entities.Parking { ParkingId = parkingId };

            _parkingRepositoryMock.Setup(repo => repo.GetById(parkingId))
                .ReturnsAsync(parking);

            _bookingRepositoryMock.Setup(repo => repo.GetAllBookingByParkingIdVer2Method(parkingId, request.PageNo, request.PageSize))
                .ReturnsAsync((List<Domain.Entities.Booking>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Không tìm thấy.");
            result.StatusCode.ShouldBe(404);
        }
        [Fact]
        public async Task Handle_BookingsFound_ShouldReturnSuccess()
        {
            // Arrange
            var parkingId = 1;
            var request = new GetAllBookingByParkingIdQuery { ParkingId = parkingId, PageNo = 1, PageSize = 10 };

            var parking = new Domain.Entities.Parking { ParkingId = parkingId };

            var bookings = new List<Domain.Entities.Booking>
            {
                new Domain.Entities.Booking
                {
                    BookingId = 1,
                    User = new User { UserId = 100, Name = "John Doe" },
                    VehicleInfor = new VehicleInfor { VehicleInforId = 200, LicensePlate = "ABC-123" },
                    BookingDetails = new List<BookingDetails>
                    {
                        new BookingDetails
                        {
                            BookingDetailsId = 10,
                            TimeSlot = new TimeSlot
                            {
                                Parkingslot = new ParkingSlot
                                {
                                    Floor = new Floor
                                    {
                                        Parking = parking
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _parkingRepositoryMock.Setup(repo => repo.GetById(parkingId))
                .ReturnsAsync(parking);

            _bookingRepositoryMock.Setup(repo => repo.GetAllBookingByParkingIdVer2Method(parkingId, request.PageNo, request.PageSize))
                .ReturnsAsync(bookings);

            _mapperMock.Setup(mapper => mapper.Map<BookingForGetAllBookingByParkingIdResponse>(It.IsAny<Domain.Entities.Booking>()))
                .Returns((Domain.Entities.Booking source) => new BookingForGetAllBookingByParkingIdResponse { BookingId = source.BookingId });

            _mapperMock.Setup(mapper => mapper.Map<FloorDtoForAdmin>(It.IsAny<Floor>()))
                .Returns((Floor source) => new FloorDtoForAdmin { FloorId = source.FloorId });

            _mapperMock.Setup(mapper => mapper.Map<ParkingDtoForAdmin>(It.IsAny<Domain.Entities.Parking>()))
                .Returns((Domain.Entities.Parking source) => new ParkingDtoForAdmin { ParkingId = source.ParkingId });

            _mapperMock.Setup(mapper => mapper.Map<SlotDtoForAdmin>(It.IsAny<ParkingSlot>()))
                .Returns((ParkingSlot source) => new SlotDtoForAdmin { ParkingSlotId = source.ParkingSlotId });

            _mapperMock.Setup(mapper => mapper.Map<UserForGetAllBookingByParkingIdResponse>(It.IsAny<User>()))
                .Returns((User source) => new UserForGetAllBookingByParkingIdResponse { UserId = source.UserId });

            _mapperMock.Setup(mapper => mapper.Map<VehicleForGetAllBookingByParkingIdResponse>(It.IsAny<VehicleInfor>()))
                .Returns((VehicleInfor source) => new VehicleForGetAllBookingByParkingIdResponse { VehicleInforId = source.VehicleInforId });



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.Count().ShouldBe(1);
        }
    }
}

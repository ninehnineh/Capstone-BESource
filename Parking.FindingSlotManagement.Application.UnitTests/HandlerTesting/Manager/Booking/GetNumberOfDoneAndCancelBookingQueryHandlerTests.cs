using Firebase.Auth;
using FirebaseAdmin.Auth;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBooking;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetNumberOfDoneAndCancelBookingQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetNumberOfDoneAndCancelBookingQueryHandler _handler;
        public GetNumberOfDoneAndCancelBookingQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetNumberOfDoneAndCancelBookingQueryHandler(_bookingRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidRequestWithBookings_ShouldReturnNumberOfDoneAndCancelBookings()
        {
            // Arrange
            var managerId = 1;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var businessProfileId = 100;
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = businessProfileId, UserId = managerId };

            var lstParking = new List<Domain.Entities.Parking>
            {
                new Domain.Entities.Parking { ParkingId = 101, BusinessId = businessProfileId },
                new Domain.Entities.Parking { ParkingId = 102, BusinessId = businessProfileId }
            };

            var doneBookingsByParkingId = new Dictionary<int, int>
            {
                { 101, 3 },
                { 102, 1 }
            };

            var cancelBookingsByParkingId = new Dictionary<int, int>
            {
                { 101, 2 },
                { 102, 0 }
            };

            var mockBookingRepository = new Mock<IBookingRepository>();
            mockBookingRepository
                .Setup(repo => repo.GetListBookingDoneOrCancelByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((parkingId, status) =>
                {
                    var count = status == Domain.Enum.BookingStatus.Done.ToString()
                        ? doneBookingsByParkingId.GetValueOrDefault(parkingId, 0)
                        : cancelBookingsByParkingId.GetValueOrDefault(parkingId, 0);
                    return Task.FromResult(count);
                });

            var userExist = new Domain.Entities.User { UserId = 1, RoleId = 1 };
            _userRepositoryMock.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync(userExist);

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(lstParking);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_ValidRequestNoBookings_ShouldReturnZero()
        {
            // Arrange
            var managerId = 1;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var businessProfileId = 100;
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = businessProfileId, UserId = managerId };

            var lstParking = new List<Domain.Entities.Parking>
        {
            new Domain.Entities.Parking { ParkingId = 101, BusinessId = businessProfileId },
            new Domain.Entities.Parking { ParkingId = 102, BusinessId = businessProfileId }
        };

            _bookingRepositoryMock
                .Setup(repo => repo.GetListBookingDoneOrCancelByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(0);

            var userExist = new Domain.Entities.User { UserId = 1, RoleId = 1 };
            _userRepositoryMock.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync(userExist);


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(lstParking);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.NumberOfDoneBooking.ShouldBe(0); // No done bookings
            result.Data.NumberOfCancelBooking.ShouldBe(0); // No cancel bookings
            result.Data.Total.ShouldBe(0); // Total done + cancel bookings
        }
        [Fact]
        public async Task Handle_InvalidManagerId_ShouldReturnError()
        {
            // Arrange
            var managerId = 999;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync((Domain.Entities.User)null);

            var handler = new GetNumberOfDoneAndCancelBookingQueryHandler(
                Mock.Of<IBookingRepository>(),
                mockUserRepository.Object,
                Mock.Of<IBusinessProfileRepository>(),
                Mock.Of<IParkingRepository>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_NonManagerAccount_ShouldReturnError()
        {
            // Arrange
            var managerId = 1;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var nonManagerUser = new Domain.Entities.User { UserId = managerId, RoleId = 2 }; // Assuming RoleId = 2 is not a manager role

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync(nonManagerUser);

            var handler = new GetNumberOfDoneAndCancelBookingQueryHandler(
                Mock.Of<IBookingRepository>(),
                mockUserRepository.Object,
                Mock.Of<IBusinessProfileRepository>(),
                Mock.Of<IParkingRepository>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Tài khoản không phải là quản lý.");
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_InvalidBusinessProfile_ShouldReturnError()
        {
            // Arrange
            var managerId = 1;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var businessProfileId = 100;


            _userRepositoryMock.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync(new Domain.Entities.User { UserId = managerId, RoleId = 1 }); // Assuming RoleId = 1 is a manager role

            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();
            mockBusinessProfileRepository.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync((Domain.Entities.BusinessProfile)null);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy thông tin doanh nghiệp.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_NoParkingInformation_ShouldReturnError()
        {
            // Arrange
            var managerId = 1;
            var request = new GetNumberOfDoneAndCancelBookingQuery { ManagerId = managerId };

            var businessProfileId = 100;
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = businessProfileId, UserId = managerId };

            _userRepositoryMock.Setup(repo => repo.GetById(managerId))
                .ReturnsAsync(new Domain.Entities.User { UserId = managerId, RoleId = 1 }); // Assuming RoleId = 1 is a manager role


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null,true))
                .ReturnsAsync(businessExist);

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(new List<Domain.Entities.Parking>()); // Empty list of parking



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy thông tin bãi giữ xe.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
    }
}

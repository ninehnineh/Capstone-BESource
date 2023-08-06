using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCard;
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
    public class GetStatisticBaseOnCardQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetStatisticBaseOnCardQueryHandler _handler;
        public GetStatisticBaseOnCardQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetStatisticBaseOnCardQueryHandler(_bookingRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidManagerId_ShouldReturnStatistics()
        {
            // Arrange
            var managerId = 101;
            var request = new GetStatisticBaseOnCardQuery { ManagerId = managerId };

            var managerExist = new User { UserId = managerId, RoleId = 1 };

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(managerExist);

            var businessExist = new Domain.Entities.BusinessProfile { UserId = managerId, BusinessProfileId = 201 };


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);

            var lstParking = new List<Domain.Entities.Parking>
            {
                new Domain.Entities.Parking { ParkingId = 301, BusinessId = 201 },
                new Domain.Entities.Parking { ParkingId = 302, BusinessId = 201 },
                // Add more parking entries as needed
            };

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(lstParking);

            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalOrdersByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id * 100)); // For simplicity, assuming the number of orders is parkingId * 100

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult((decimal)(id * 1000))); // For simplicity, assuming the revenue is parkingId * 1000

            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id * 10)); // For simplicity, assuming the total number of orders in current day is parkingId * 10

            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalWaitingOrdersByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id)); // For simplicity, assuming the total waiting orders is equal to parkingId



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
        }
        [Fact]
        public async Task Handle_InvalidManagerId_ShouldReturnError()
        {
            // Arrange
            var managerId = 999;
            var request = new GetStatisticBaseOnCardQuery { ManagerId = managerId };


            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((User)null);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_NonManagerUser_ShouldReturnError()
        {
            // Arrange
            var managerId = 101;
            var request = new GetStatisticBaseOnCardQuery { ManagerId = managerId };

            var nonManagerUser = new User { UserId = managerId, RoleId = 2 }; // Assuming RoleId 2 is not a manager


            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(nonManagerUser);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Tài khoản không phải là quản lý.");
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
        }

        // Test case for missing business profile
        [Fact]
        public async Task Handle_MissingBusinessProfile_ShouldReturnError()
        {
            // Arrange
            var managerId = 101;
            var request = new GetStatisticBaseOnCardQuery { ManagerId = managerId };

            var managerExist = new User { UserId = managerId, RoleId = 1 };


            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(managerExist);

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
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

        // Test case for no parking entries found
        [Fact]
        public async Task Handle_NoParkingEntriesFound_ShouldReturnError()
        {
            // Arrange
            var managerId = 101;
            var request = new GetStatisticBaseOnCardQuery { ManagerId = managerId };

            var managerExist = new User { UserId = managerId, RoleId = 1 };

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(managerExist);

            var businessExist = new Domain.Entities.BusinessProfile { UserId = managerId, BusinessProfileId = 201 };


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);


            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(new List<Domain.Entities.Parking>());



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

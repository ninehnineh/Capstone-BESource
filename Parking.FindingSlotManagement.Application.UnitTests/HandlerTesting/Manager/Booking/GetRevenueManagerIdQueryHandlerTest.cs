using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueManagerId;
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
    public class GetRevenueManagerIdQueryHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetRevenueManagerIdQueryHandler _handler;
        public GetRevenueManagerIdQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetRevenueManagerIdQueryHandler(_bookingRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidRequestWithWeek_ShouldReturnRevenueForWeek()
        {
            // Arrange
            var managerId = 101;
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId, Week = "current" };

            var managerExist = new Domain.Entities.User { UserId = managerId, RoleId = 1 };


            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(managerExist);

            var businessExist = new Domain.Entities.BusinessProfile { UserId = managerId, BusinessProfileId = 201 };


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);

            var currentDate = new DateTime(2023, 7, 31);
            var startDate = new DateTime(2023, 7, 24);
            var endDate = new DateTime(2023, 7, 30);

            var lstParking = new List<Domain.Entities.Parking>
        {
            new Domain.Entities.Parking { ParkingId = 301, BusinessId = 201 },
            new Domain.Entities.Parking { ParkingId = 302, BusinessId = 201 },
            // Add more parking entries as needed
        };

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(lstParking);

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByDateByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((id, date) =>
                {
                    // For simplicity, returning some dummy revenue values for each date
                    if (date == startDate)
                    {
                        return Task.FromResult(100M);
                    }
                    if (date == startDate.AddDays(1))
                    {
                        return Task.FromResult(150M);
                    }
                    // Add similar conditions for other dates in the week
                    return Task.FromResult(0M);
                });


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            // Add more assertions based on the expected revenue values for each date in the week
            // For example, result.Data should contain a list of 7 GetRevenueManagerIdResponse objects with the expected revenue values for each date.
        }
        [Fact]
        public async Task Handle_ValidRequestWithMonth_ShouldReturnRevenueForMonth()
        {
            // Arrange
            var managerId = 101;
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId, Month = "current" };

            var managerExist = new User { UserId = managerId, RoleId = 1 };


            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(managerExist);

            var businessExist = new Domain.Entities.BusinessProfile { UserId = managerId, BusinessProfileId = 201 };


            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessExist);

            var currentDate = new DateTime(2023, 7, 31);
            var year = currentDate.Year;
            var month = currentDate.Month;
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var lstParking = new List<Domain.Entities.Parking>
        {
            new Domain.Entities.Parking { ParkingId = 301, BusinessId = 201 },
            new Domain.Entities.Parking { ParkingId = 302, BusinessId = 201 },
            // Add more parking entries as needed
        };

            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, null, true))
                .ReturnsAsync(lstParking);

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByDateByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((id, date) =>
                {
                    // For simplicity, returning some dummy revenue values for each date
                    if (date == new DateTime(2023, 7, 1))
                    {
                        return Task.FromResult(200M);
                    }
                    if (date == new DateTime(2023, 7, 2))
                    {
                        return Task.FromResult(300M);
                    }
                    // Add similar conditions for other dates in the month
                    return Task.FromResult(0M);
                });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            // Add more assertions based on the expected revenue values for each date in the month
            // For example, result.Data should contain a list of 'daysInMonth' GetRevenueManagerIdResponse objects with the expected revenue values for each date.
        }
        [Fact]
        public async Task Handle_InvalidManagerId_ShouldReturnError()
        {
            // Arrange
            var managerId = 999;
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId };


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
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId };

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
        [Fact]
        public async Task Handle_MissingBusinessProfile_ShouldReturnError()
        {
            // Arrange
            var managerId = 101;
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId };

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
        [Fact]
        public async Task Handle_NoParkingEntriesFound_ShouldReturnError()
        {
            // Arrange
            var managerId = 101;
            var request = new GetRevenueManagerIdQuery { ManagerId = managerId };

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

using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class GetListParkingByManagerIdQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly GetListParkingByManagerIdQueryHandler _handler;
        public GetListParkingByManagerIdQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository> ();
            _handler = new GetListParkingByManagerIdQueryHandler(_parkingRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidManagerIdAndRole_ShouldReturnListOfParkings()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId

            var mockParkingRepository = new Mock<IParkingRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();

            var managerUser = new User
            {
                UserId = managerId,
                RoleId = 1 // Assuming RoleId 1 is for managers
            };

            var businessProfile = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId = 1,
                UserId = managerId
            };

            var parkings = new List<Domain.Entities.Parking>
            {
                new Domain.Entities.Parking { ParkingId = 1, BusinessId = 1, IsActive = true },
                new Domain.Entities.Parking { ParkingId = 2, BusinessId = 1, IsActive = true }
                // Add more parkings as needed for testing pagination
            };

            mockUserRepository.Setup(repo => repo.GetById(managerId)).ReturnsAsync(managerUser);
            mockBusinessProfileRepository.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true)).ReturnsAsync(businessProfile);
            mockParkingRepository.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(),
                null,
                It.IsAny<Expression<Func<Domain.Entities.Parking, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(parkings);

            var handler = new GetListParkingByManagerIdQueryHandler(
                mockParkingRepository.Object,
                mockUserRepository.Object,
                mockBusinessProfileRepository.Object);

            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(parkings.Count);
        }
        [Fact]
        public async Task Handle_ManagerNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int managerId = 1; // Replace with a non-existing managerId

            var mockParkingRepository = new Mock<IParkingRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();

            mockUserRepository.Setup(repo => repo.GetById(managerId)).ReturnsAsync((User)null);

            var handler = new GetListParkingByManagerIdQueryHandler(
                mockParkingRepository.Object,
                mockUserRepository.Object,
                mockBusinessProfileRepository.Object);

            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản quản lý.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }
        [Fact]
        public async Task Handle_ManagerNotCorrectRole_ShouldReturnBadRequest()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId with incorrect role

            var mockParkingRepository = new Mock<IParkingRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();

            var managerUser = new User
            {
                UserId = managerId,
                RoleId = 2 // Assuming RoleId 2 is for non-managers
            };

            mockUserRepository.Setup(repo => repo.GetById(managerId)).ReturnsAsync(managerUser);

            var handler = new GetListParkingByManagerIdQueryHandler(
                mockParkingRepository.Object,
                mockUserRepository.Object,
                mockBusinessProfileRepository.Object);

            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("tài khoản không phải là quản lý.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }
        [Fact]
        public async Task Handle_BusinessNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId
            int businessProfileId = 1; // Replace with a non-existing businessProfileId


            var managerUser = new User
            {
                UserId = managerId,
                RoleId = 1 // Assuming RoleId 1 is for managers
            };

            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).ReturnsAsync(managerUser);
            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true)).ReturnsAsync((Domain.Entities.BusinessProfile)null);


            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản doanh nghiệp.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }
        [Fact]
        public async Task Handle_ParkingNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId
            int businessProfileId = 1; // Replace with a valid businessProfileId


            var managerUser = new User
            {
                UserId = managerId,
                RoleId = 1 // Assuming RoleId 1 is for managers
            };

            var businessProfile = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId = businessProfileId,
                UserId = managerId
            };

            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).ReturnsAsync(managerUser);
            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny < Expression < Func<Domain.Entities.BusinessProfile, bool> >> (), null, true)).ReturnsAsync(businessProfile);
            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(),
                null,
                It.IsAny<Expression<Func<Domain.Entities.Parking, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync((IEnumerable<Domain.Entities.Parking>)null);

            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId

            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).Throws(new Exception("Simulated exception"));


            var query = new GetListParkingByManagerIdQuery
            {
                ManagerId = managerId
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}

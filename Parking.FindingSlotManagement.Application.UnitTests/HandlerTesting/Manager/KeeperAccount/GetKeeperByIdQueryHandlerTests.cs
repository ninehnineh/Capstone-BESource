using Microsoft.Build.Execution;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetKeeperById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.KeeperAccount
{
    public class GetKeeperByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetKeeperByIdQueryHandler _handler;
        public GetKeeperByIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetKeeperByIdQueryHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingUser_ShouldReturnUser()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userIdToGet = 1; // Replace with the user ID you want to retrieve
            var query = new GetKeeperByIdQuery
            {
                UserId = userIdToGet
            };

            var existingUser = new User
            {
                UserId = userIdToGet,
                Role = new Role { RoleId = 1 },
                Parking = new Domain.Entities.Parking { ParkingId = 1 },
                // Add other properties of the existing user
            };

            var handler = new GetKeeperByIdQueryHandler(mockUserRepository.Object);

            mockUserRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>())).ReturnsAsync(existingUser);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldBeOfType<GetKeeperByIdResponse>();
            // Perform additional assertions to validate the response data.
            // For example:
            result.Data.UserId.ShouldBe(userIdToGet);

            // Additional assertions if needed
            mockUserRepository.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);
        }
        [Fact]
        public async Task Handle_NonExistingUser_ShouldReturnNotFound()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userIdToGet = 1; // Replace with the user ID for non-existing user
            var query = new GetKeeperByIdQuery
            {
                UserId = userIdToGet
            };

            var handler = new GetKeeperByIdQueryHandler(mockUserRepository.Object);

            mockUserRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>())).ReturnsAsync((User)null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.Data.ShouldBeNull();

            // Additional assertions if needed
            mockUserRepository.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {

            var userIdToGet = 1; // Replace with the user ID for testing exception scenario
            var query = new GetKeeperByIdQuery
            {
                UserId = userIdToGet
            };


            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>())).Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);
        }
    }
}

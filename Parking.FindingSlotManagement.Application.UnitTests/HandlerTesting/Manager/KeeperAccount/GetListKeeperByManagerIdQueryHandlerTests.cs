using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId;
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
    public class GetListKeeperByManagerIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetListKeeperByManagerIdQueryHandler _handler;
        public GetListKeeperByManagerIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetListKeeperByManagerIdQueryHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingManagerWithKeepers_ShouldReturnKeeperList()
        {

            var managerId = 1; // Replace with the manager ID for testing
            var query = new GetListKeeperByManagerIdQuery
            {
                ManagerId = managerId,
                PageNo = 1,
                PageSize = 10 // Replace with the desired page size for testing
            };

            var existingManager = new User
            {
                UserId = managerId,
                RoleId = 1, // Assuming 1 is the role ID for managers
                            // Add other properties of the existing manager
            };

            var existingKeepers = new List<User>
            {
                new User { UserId = 1, RoleId = 1 },
                new User { UserId = 2, RoleId = 1 },
                // Add more keepers as needed
            };



            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).ReturnsAsync(existingManager);
            _userRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<Expression<Func<User, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(existingKeepers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldBeOfType<List<GetListKeeperByManagerIdResponse>>();



            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(managerId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<Expression<Func<User, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task Handle_NonExistingManager_ShouldReturnNotFound()
        {

            var managerId = 1; // Replace with the non-existing manager ID for testing
            var query = new GetListKeeperByManagerIdQuery
            {
                ManagerId = managerId,
                PageNo = 1,
                PageSize = 10 // Replace with the desired page size for testing
            };


            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản quản lý.");
            result.Data.ShouldBeNull();

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(managerId), Times.Once);
        }
        [Fact]
        public async Task Handle_ExistingManagerWithNoKeepers_ShouldReturnEmptyList()
        {
            // Arrange
            var managerId = 1; // Replace with the manager ID for testing
            var query = new GetListKeeperByManagerIdQuery
            {
                ManagerId = managerId,
                PageNo = 1,
                PageSize = 10 // Replace with the desired page size for testing
            };

            var existingManager = new User
            {
                UserId = managerId,
                RoleId = 1, // Assuming 1 is the role ID for managers
                            // Add other properties of the existing manager
            };


            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).ReturnsAsync(existingManager);
            _userRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<Expression<Func<User, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(new List<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(managerId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<Expression<Func<User, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {

            var managerId = 1; // Replace with the manager ID for testing
            var query = new GetListKeeperByManagerIdQuery
            {
                ManagerId = managerId,
                PageNo = 1,
                PageSize = 10 // Replace with the desired page size for testing
            };


            _userRepositoryMock.Setup(repo => repo.GetById(managerId)).Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(managerId), Times.Once);
        }
    }
}

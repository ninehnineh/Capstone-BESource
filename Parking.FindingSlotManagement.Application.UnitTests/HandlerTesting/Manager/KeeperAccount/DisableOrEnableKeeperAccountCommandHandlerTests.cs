using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.DisableOrEnableKeeperAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.KeeperAccount
{
    public class DisableOrEnableKeeperAccountCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DisableOrEnableKeeperAccountCommandHandler _handler;
        public DisableOrEnableKeeperAccountCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DisableOrEnableKeeperAccountCommandHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_DisableActiveAccount_ShouldDisableAccount()
        {
            // Arrange
            var userIdToDisable = 1; // Replace with the user ID you want to disable
            var command = new DisableOrEnableKeeperAccountCommand
            {
                UserId = userIdToDisable
            };

            var existingUser = new User
            {
                UserId = userIdToDisable,
                IsActive = true // Assuming the user account is active
                                // Add other properties of the existing user
            };



            _userRepositoryMock.Setup(repo => repo.GetById(userIdToDisable)).ReturnsAsync(existingUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            existingUser.IsActive?.ShouldBeFalse();

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(userIdToDisable), Times.Once);

            _userRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_AccountNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            var userIdToDisableOrEnable = 1; // Replace with the user ID for non-existing user
            var command = new DisableOrEnableKeeperAccountCommand
            {
                UserId = userIdToDisableOrEnable
            };

            _userRepositoryMock.Setup(repo => repo.GetById(userIdToDisableOrEnable)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(userIdToDisableOrEnable), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_EnableInactiveAccount_ShouldEnableAccount()
        {

            var userIdToEnable = 1; // Replace with the user ID you want to enable
            var command = new DisableOrEnableKeeperAccountCommand
            {
                UserId = userIdToEnable
            };

            var existingUser = new User
            {
                UserId = userIdToEnable,
                IsActive = false // Assuming the user account is inactive
                                 // Add other properties of the existing user
            };


            _userRepositoryMock.Setup(repo => repo.GetById(userIdToEnable)).ReturnsAsync(existingUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            existingUser.IsActive?.ShouldBeTrue();

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(userIdToEnable), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            var userIdToDisableOrEnable = 1; // Replace with the user ID for testing exception scenario
            var command = new DisableOrEnableKeeperAccountCommand
            {
                UserId = userIdToDisableOrEnable
            };

            _userRepositoryMock.Setup(repo => repo.GetById(userIdToDisableOrEnable)).Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
            // You can also check the specific exception message if needed.

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetById(userIdToDisableOrEnable), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
    }
}

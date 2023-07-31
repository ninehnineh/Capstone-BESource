using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.DeviceToken.Commands.UpdateDeviceToken;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.DeviceToken
{
    public class UpdateDeviceTokenCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateDeviceTokenCommandHandler _handler;
        public UpdateDeviceTokenCommandHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateDeviceTokenCommandHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new UpdateDeviceTokenCommand { UserId = 1, Devicetoken = "device-token-123" };
            _userRepositoryMock.Setup(repo => repo.GetById(1))
                               .ReturnsAsync((Domain.Entities.User)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
        [Fact]
        public async Task Handle_WhenDeviceTokenIsNullForUser_SavesDeviceTokenAndReturnsSuccessResponse()
        {
            // Arrange
            var request = new UpdateDeviceTokenCommand { UserId = 1, Devicetoken = "device-token-123" };
            var user = new Domain.Entities.User { UserId = 1, Devicetoken = null };
            _userRepositoryMock.Setup(repo => repo.GetById(1))
                               .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");

            // Check that the device token was saved for the user
            user.Devicetoken.ShouldBe(request.Devicetoken);
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_WhenDeviceTokenMatchesExistingValue_ReturnsSuccessResponseWithoutSaving()
        {
            // Arrange
            var request = new UpdateDeviceTokenCommand { UserId = 1, Devicetoken = "device-token-123" };
            var user = new Domain.Entities.User { UserId = 1, Devicetoken = "device-token-123" };
            _userRepositoryMock.Setup(repo => repo.GetById(1))
                               .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");

            // Check that the device token was not saved for the user
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_WhenDeviceTokenDiffersFromExistingValue_SavesDeviceTokenAndReturnsSuccessResponse()
        {
            // Arrange
            var request = new UpdateDeviceTokenCommand { UserId = 1, Devicetoken = "new-device-token" };
            var user = new Domain.Entities.User { UserId = 1, Devicetoken = "old-device-token" };
            _userRepositoryMock.Setup(repo => repo.GetById(1))
                               .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");

            // Check that the new device token was saved for the user
            user.Devicetoken.ShouldBe(request.Devicetoken);
            _userRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
    }
}

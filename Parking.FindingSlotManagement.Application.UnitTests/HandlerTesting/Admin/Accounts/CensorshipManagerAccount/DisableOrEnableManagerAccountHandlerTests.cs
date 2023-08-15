using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.DeleteCensorshipManagerAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.CensorshipManagerAccount
{
    public class DisableOrEnableManagerAccountHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly DisableOrEnableCensorshipHandler _handler;

        public DisableOrEnableManagerAccountHandlerTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _handler = new DisableOrEnableCensorshipHandler(_mockAccountRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableManagerAccountCommand
            {
                UserId = 1
            };

            var managerToDelete = new User
            {
                UserId = request.UserId,
                IsActive = true,
            };

            _mockAccountRepository.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(managerToDelete);

            _mockAccountRepository.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _mockAccountRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockAccountRepository.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Account_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableManagerAccountCommand
            {
                UserId = 1
            };

            _mockAccountRepository.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _mockAccountRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockAccountRepository.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableManagerAccountCommand
            {
                UserId = 1
            };

            _mockAccountRepository.Setup(x => x.GetById(request.UserId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _mockAccountRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockAccountRepository.Verify(x => x.Save(), Times.Never);
        }
    }
}

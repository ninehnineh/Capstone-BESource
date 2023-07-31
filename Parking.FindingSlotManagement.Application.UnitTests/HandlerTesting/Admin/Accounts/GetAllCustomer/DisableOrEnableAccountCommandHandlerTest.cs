using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.DeleteCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Commands.DisableOrEnableAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.GetAllCustomer
{
    public class DisableOrEnableAccountCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly DisableOrEnableAccountCommandHandler _handler;
        public DisableOrEnableAccountCommandHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new DisableOrEnableAccountCommandHandler(_mockUserRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableAccountCommand
            {
                UserId = 1
            };

            var userToDelete = new User
            {
                UserId = request.UserId,
                IsActive = true,
            };

            _mockUserRepository.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(userToDelete);

            _mockUserRepository.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _mockUserRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockUserRepository.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_User_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableAccountCommand
            {
                UserId = 1
            };

            _mockUserRepository.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _mockUserRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockUserRepository.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableAccountCommand
            {
                UserId = 1
            };

            _mockUserRepository.Setup(x => x.GetById(request.UserId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _mockUserRepository.Verify(x => x.GetById(request.UserId), Times.Once);
            _mockUserRepository.Verify(x => x.Save(), Times.Never);
        }
    }
}

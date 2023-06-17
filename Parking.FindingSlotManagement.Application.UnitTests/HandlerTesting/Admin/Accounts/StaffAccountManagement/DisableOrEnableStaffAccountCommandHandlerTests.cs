using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.DisableOrEnableStaffAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.StaffAccountManagement
{
    public class DisableOrEnableStaffAccountCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DisableOrEnableStaffAccountCommandHandler _handler;
        public DisableOrEnableStaffAccountCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DisableOrEnableStaffAccountCommandHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableStaffAccountCommand
            {
                UserId = 1
            };

            var staffToDelete = new User
            {
                UserId = request.UserId,
                IsActive = true,
            };

            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(staffToDelete);

            _userRepositoryMock.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _userRepositoryMock.Verify(x => x.GetById(request.UserId), Times.Once);
            _userRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Account_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableStaffAccountCommand
            {
                UserId = 1
            };

            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _userRepositoryMock.Verify(x => x.GetById(request.UserId), Times.Once);
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableStaffAccountCommand
            {
                UserId = 1
            };

            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _userRepositoryMock.Verify(x => x.GetById(request.UserId), Times.Once);
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}

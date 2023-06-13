using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdatePasswordForStaff;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.StaffAccountManagement
{
    public class UpdatePasswordForStaffCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly UpdatePasswordForStaffCommandHandler _handler;
        public UpdatePasswordForStaffCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _handler = new UpdatePasswordForStaffCommandHandler(_userRepositoryMock.Object, _emailServiceMock.Object);
        }
        [Fact]
        public async Task UpdatePasswordForStaffCommandHandler_Should_Update_Password_Successfully()
        {
            // Arrange
            var request = new UpdatePasswordForStaffCommand
            {
                UserId = 10,
                NewPassword = "456789"
            };
            var cancellationToken = new CancellationToken();
            var account = new User
            {
                UserId = request.UserId,
                Name = "Old Name",
                Email = "old.email@example.com",
                Phone = "987654321",
                Avatar = "oldavatarurl",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = "Female",
                RoleId = 4,

            };
            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(account);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            _userRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task UpdatePasswordForStaffCommandHandler_Should_Return_Not_Found_If_Account_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdatePasswordForStaffCommand
            {
                UserId = 2000
            };
            var cancellationToken = new CancellationToken();
            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null!);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task UpdatePasswordForStaffCommandHandler_Should_Return_Bad_Request_If_Role_Of_Account_Does_Not_A_Staff()
        {
            // Arrange
            var request = new UpdatePasswordForStaffCommand
            {
                UserId = 10,
                NewPassword = "456789"
            };
            var cancellationToken = new CancellationToken();
            var account = new User
            {
                UserId = request.UserId,
                Name = "Old Name",
                Email = "old.email@example.com",
                Phone = "987654321",
                Avatar = "oldavatarurl",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = "Female",
                RoleId = 3,

            };
            _userRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(account);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Tài khoản không phải staff nên không thể cấp lại mật khẩu.");
            response.StatusCode.ShouldBe(400);
            response.Count.ShouldBe(0);
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}

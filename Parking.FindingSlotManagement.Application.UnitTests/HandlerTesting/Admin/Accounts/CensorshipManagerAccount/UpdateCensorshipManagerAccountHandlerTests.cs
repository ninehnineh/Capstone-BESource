using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.UpdateCensorshipManagerAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.CensorshipManagerAccount
{
    public class UpdateCensorshipManagerAccountHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly UpdateCensorshipManagerAccountCommandHandler _handler;

        public UpdateCensorshipManagerAccountHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new UpdateCensorshipManagerAccountCommandHandler(_accountRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateCensorshipManagerAccountCommandHandler_Should_Update_Account_Successfully()
        {
            // Arrange
            var request = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "New Name",
                Email = "new.email@example.com",
                Password = "newPassword",
                Phone = "123456789",
                Avatar = "newavatarurl",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male"
            };
            var cancellationToken = new CancellationToken();
            var account = new User
            {
                UserId = request.UserId,
                Name = "Old Name",
                Email = "old.email@example.com",
                Password = "oldPassword",
                Phone = "987654321",
                Avatar = "oldavatarurl",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = "Female"
            };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(account);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            account.Name.ShouldBe(request.Name);
            account.Email.ShouldBe(request.Email);
            account.Password.ShouldBe(request.Password);
            account.Phone.ShouldBe(request.Phone);
            account.Avatar.ShouldBe(request.Avatar);
            account.DateOfBirth.ShouldBe(request.DateOfBirth);
            account.Gender.ShouldBe(request.Gender);
            _accountRepositoryMock.Verify(x => x.Update(account), Times.Once);
        }
        [Fact]
        public async Task UpdateCensorshipManagerAccountCommandHandler_Should_Return_Not_Found_If_Account_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 2000
            };
            var cancellationToken = new CancellationToken();
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _accountRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }
        [Fact]
        public async Task UpdateCensorshipManagerAccountCommandHandler_Should_Return_Email_Already_Exists()
        {
            // Arrange
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Linh Đỗ",
                Email = "chinh.truongcong202@gmail.com", // This email already exists
                Password = "newpassword",
                Phone = "1234567890",
                Avatar = "https://example.com/newavatar",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male"
            };
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetById(command.UserId))
                .ReturnsAsync(new User
                {
                    UserId = command.UserId,
                    Email = "linhdase151281@fpt.edu.vn" // Existing email for the account
                });
            accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync(new User
                {
                    UserId = 10,
                    Email = command.Email // Email already exists for another account
                });
            var handler = new UpdateCensorshipManagerAccountCommandHandler(accountRepositoryMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Email đã tồn tại. Vui lòng nhập email khác!!!");
            accountRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }
    }
}

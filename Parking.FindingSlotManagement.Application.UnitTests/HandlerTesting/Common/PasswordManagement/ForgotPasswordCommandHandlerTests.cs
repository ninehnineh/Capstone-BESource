using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.PasswordManagement.Commands.ForgotPassword;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.PasswordManagement
{
    public class ForgotPasswordCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly ForgotPasswordCommandValidation _validator;
        private readonly ForgotPasswordCommandHandler _handler;
        public ForgotPasswordCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new ForgotPasswordCommandValidation();
            _handler = new ForgotPasswordCommandHandler(_accountRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidEmail_ShouldResetPassword()
        {
            // Arrange
            var email = "john@example.com";
            var newPassword = "newpassword";
            var account = new User { UserId = 1, Email = email, IsActive = true, IsCensorship = true };
            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync(account);
            _accountRepositoryMock.Setup(x => x.GetById(account.UserId)).ReturnsAsync(account);

            var command = new ForgotPasswordCommand { Email = email, NewPassword = newPassword };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            account.PasswordHash.ShouldNotBeNull();
            account.PasswordSalt.ShouldNotBeNull();
            _accountRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_WithNonExistingEmail_ShouldReturnError()
        {
            // Arrange
            var email = "nonexisting@example.com";
            var newPassword = "newpassword";

            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync((User)null);

            var command = new ForgotPasswordCommand { Email = email, NewPassword = newPassword };


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            _accountRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_WithInactiveAccount_ShouldReturnError()
        {
            // Arrange
            var email = "john@example.com";
            var newPassword = "newpassword";
            var account = new User { UserId = 1, Email = email, IsActive = false, IsCensorship = false };

            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync(account);

            var command = new ForgotPasswordCommand { Email = email, NewPassword = newPassword };


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tài khoản của bạn đang trong quá trình kiểm duyệt hoặc bị hệ thống Ban.");
            _accountRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_With_Still_Censoring_Account_ShouldReturnError()
        {
            // Arrange
            var email = "john@example.com";
            var newPassword = "newpassword";
            var account = new User { UserId = 1, Email = email, IsActive = true, IsCensorship = false };

            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync(account);

            var command = new ForgotPasswordCommand { Email = email, NewPassword = newPassword };


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tài khoản của bạn đang trong quá trình kiểm duyệt hoặc bị hệ thống Ban.");
            _accountRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public void Email_ShouldNotBeEmpty()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "",
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Email_ShouldNotBeNull()
        {
            var command = new ForgotPasswordCommand
            {
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Email_ShouldNotExceedMaximumLength()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "john@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.comjohn@example.com",
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void NewPassword_ShouldNotBeEmpty()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "john@example.com",
                NewPassword = ""
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
        [Fact]
        public void NewPassword_ShouldNotBeNull()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "john@example.com",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
        [Fact]
        public void NewPassword_ShouldNotExceedMaximumLength()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "john@example.com",
                NewPassword = "newPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPasswordnewPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}

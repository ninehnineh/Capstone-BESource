using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.Accounts.StaffAccountManagement.Commands.UpdatePasswordByStaffId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.Account.StaffAccountManagement
{
    public class UpdatePasswordByStaffIdCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly UpdatePasswordByStaffIdCommandValidation _validator;
        private readonly UpdatePasswordByStaffIdCommandHandler _handler;
        public UpdatePasswordByStaffIdCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new UpdatePasswordByStaffIdCommandValidation();
            _handler = new UpdatePasswordByStaffIdCommandHandler(_accountRepositoryMock.Object);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [Fact]
        public async Task Handle_WhenUpdateSuccessful_ReturnsSuccess()
        {
            // Arrange
            CreatePasswordHash("oldpassword", out byte[] passwordHash, out byte[] passwordSalt);
            var account = new User { ManagerId = 1, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
            _accountRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(account);
            var command = new UpdatePasswordByStaffIdCommand { UserId = 1, OldPassword = "oldpassword", NewPassword = "newpassword" };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");
            account.PasswordHash.ShouldNotBeNull();
            account.PasswordSalt.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_WhenManagerNotFound_ReturnsError()
        {
            // Arrange
            _accountRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);
            var command = new UpdatePasswordByStaffIdCommand { UserId = 1 };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
        [Fact]
        public async Task Handle_WhenOldPasswordIncorrect_ReturnsError()
        {
            // Arrange
            CreatePasswordHash("oldpassword", out byte[] passwordHash, out byte[] passwordSalt);
            _accountRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(new User { ManagerId = 1, PasswordHash = passwordHash, PasswordSalt = passwordSalt });
            var command = new UpdatePasswordByStaffIdCommand { UserId = 1, OldPassword = "wrongpassword", NewPassword = "newpassword" };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Mật khẩu cũ không đúng. Vui lòng nhập lại!!!");
        }
        [Fact]
        public void OldPassword_ShouldNotBeEmpty()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                OldPassword = "",
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        }
        [Fact]
        public void OldPassword_ShouldNotBeNull()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        }
        [Fact]
        public void OldPassword_ShouldNotExceedMaximumLength()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                OldPassword = "oldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPassword",
                NewPassword = "newPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        }
        [Fact]
        public void NewPassword_ShouldNotBeEmpty()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                OldPassword = "oldPassword",
                NewPassword = ""
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
        [Fact]
        public void NewPassword_ShouldNotBeNull()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                OldPassword = "oldPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
        [Fact]
        public void NewPassword_ShouldNotExceedMaximumLength()
        {
            var command = new UpdatePasswordByStaffIdCommand
            {
                UserId = 1,
                OldPassword = "oldPassword",
                NewPassword = "oldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPasswordoldPassword"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}

using FluentValidation.TestHelper;
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
        private readonly UpdateCensorshipManagerAccountCommandValidation _validator;

        public UpdateCensorshipManagerAccountHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new UpdateCensorshipManagerAccountCommandHandler(_accountRepositoryMock.Object);
            _validator = new UpdateCensorshipManagerAccountCommandValidation();
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
                .ReturnsAsync((User)null!);

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
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê  lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem",
                Email = "nle549220@gmail.com",
                Password = "password",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Email_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
                Email = "loremmfsadmfdmsfifjdsnmnksdakfgsaosfojdksfjsdfsadkfsdklafkljsdjkf.oldslfldsaflsdalfldsflsdfldslfldsfdsfnsdjffvsdafjdsvjk@gmail.com",
                Password = "password",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Phone_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Password = "password",
                Phone = "0123456789113",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }
        [Fact]
        public void DateOfBirth_MustLessThanCurrentDate()
        {
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Password = "password",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.UtcNow.AddHours(7).AddDays(1),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
        [Fact]
        public void Gender_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCensorshipManagerAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Password = "password",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female lorem lorem lorem"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }
    }
}

using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.StaffAccountManagement.Commands.CreateNewStaffAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Account.StaffAccountManagement
{
    public class CreateNewStaffAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly CreateNewStaffAccountCommandValidattion _validator;
        private readonly CreateNewStaffAccountCommandHandler _handler;
        public CreateNewStaffAccountCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _validator = new CreateNewStaffAccountCommandValidattion();
            _handler = new CreateNewStaffAccountCommandHandler(_accountRepositoryMock.Object, _emailServiceMock.Object);
        }
        [Fact]
        public async Task Handle_WithExistingEmail_ShouldReturnErrorMessage()
        {
            // Arrange
            var command = new CreateNewStaffAccountCommand { Email = "existing.email@example.com" };
            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
               .ReturnsAsync(new User { Email = command.Email });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Email đã tồn tại. Vui lòng nhập email khác!!!");
        }
        [Fact]
        public async Task Handle_WhenEmailDoesNotExist_CreatesNewAccountAndSendsEmail()
        {
            // Arrange
            var request = new CreateNewStaffAccountCommand { Name = "Nguyên Lê", Email = "nle549220@gmail.com", Phone = "0123456789", Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png", DateOfBirth = DateTime.Parse("2000-01-10"), Gender = "Female" };
            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
                .ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");

        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Email_ShouldNotBeEmpty()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Email_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Email_ShouldBeValidEmailAddress()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "notavalidemailaddress",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Email_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "loremmfsadmfdmsfifjdsnmnksdakfgsaosfojdksfjsdfsadkfsdklafkljsdjkf.oldslfldsaflsdalfldsflsdfldslfldsfdsfnsdjffvsdafjdsvjk@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void Phone_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }
        [Fact]
        public void Phone_ShouldBeNumbers()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "aaaaaaaaaa",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }
        [Fact]
        public void Phone_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789113",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }
        [Fact]
        public void Avatar_ShouldNotBeEmpty()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Avatar);
        }

        [Fact]
        public void Avatar_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Avatar);
        }
        [Fact]
        public void DateOfBirth_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
        [Fact]
        public void DateOfBirth_ShouldNotLessThanCurrentDate()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.UtcNow.AddHours(7).AddDays(1),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
        [Fact]
        public void Gender_ShouldNotBeEmpty()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = ""
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }

        [Fact]
        public void Gender_ShouldNotBeNull()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }
        [Fact]
        public void Gender_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewStaffAccountCommand
            {
                Name = "Nguyên Lê",
                Email = "nle549220@gmail.com",
                Phone = "0123456789",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Femaleloremloremloremloremlorem"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }
    }
}

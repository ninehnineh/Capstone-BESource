using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Commands.UpdateCustomerProfileById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Account
{
    public class UpdateCustomerProfileByIdCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateCustomerProfileByIdCommandHandler _handler;
        private readonly UpdateCustomerProfileByIdValidation _validator;
        public UpdateCustomerProfileByIdCommandHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new UpdateCustomerProfileByIdValidation();
            _handler = new UpdateCustomerProfileByIdCommandHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "Male",
                Address = "123 Main St"
            };
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
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
        public async Task Handle_WhenUserExists_UpdatesUserInformationAndReturnsSuccessResponse()
        {
            // Arrange
            var request = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "Male",
                Address = "123 Main St"
            };
            var existingUser = new Domain.Entities.User { UserId = 1 };
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                               .ReturnsAsync(existingUser);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");

            // Verify that the user information is updated correctly
            existingUser.Name.ShouldBe("John Doe");
            existingUser.Avatar.ShouldBe("avatar_url");
            existingUser.DateOfBirth.ShouldBe(new DateTime(1990, 01, 01));
            existingUser.Gender.ShouldBe("Male");
            existingUser.Address.ShouldBe("123 Main St");

            // Verify that the Update method was called with the correct user
            _userRepositoryMock.Verify(repo => repo.Update(existingUser), Times.Once);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn DoeJohn Doe",
                Avatar = "avatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "Male",
                Address = "123 Main St"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void DateOfBirth_ShouldNotGreaterThanCurrentDate()
        {
            var command = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_url",
                DateOfBirth = DateTime.Now.AddDays(1),
                Gender = "Male",
                Address = "123 Main St"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
        [Fact]
        public void Gender_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "MGenderGenderGenderGenderGenderGenderGenderGenderGenderGenderGenderGender",
                Address = "123 Main St"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }
        [Fact]
        public void Avatar_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "Gender",
                Address = "123 Main St"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Avatar);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateCustomerProfileByIdCommand
            {
                UserId = 1,
                Name = "John Doe",
                Avatar = "avatar_url",
                DateOfBirth = new DateTime(1990, 01, 01),
                Gender = "Gender",
                Address = "123 Main Stavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_urlavatar_url"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
    }
}

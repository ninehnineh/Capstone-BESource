using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdateStaffAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.StaffAccountManagement
{
    public class UpdateStaffAccountCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateStaffAccountCommandHandler _handler;
        private readonly UpdateStaffAccountCommandValidation _validator;
        public UpdateStaffAccountCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new UpdateStaffAccountCommandValidation();
            _handler = new UpdateStaffAccountCommandHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateStaffAccountCommandHandler_Should_Update_Account_Successfully()
        {
            // Arrange
            var request = new UpdateStaffAccountCommand
            {
                UserId = 10,
                Name = "New Name",
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
                Avatar = "oldavatarurl",
                DateOfBirth = new DateTime(1980, 1, 1),
                Gender = "Female"
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
            account.Name.ShouldBe(request.Name);
            account.Avatar.ShouldBe(request.Avatar);
            account.DateOfBirth.ShouldBe(request.DateOfBirth);
            account.Gender.ShouldBe(request.Gender);
            _userRepositoryMock.Verify(x => x.Update(account), Times.Once);
        }
        [Fact]
        public async Task UpdateStaffAccountCommandHandler_Should_Return_Not_Found_If_Account_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateStaffAccountCommand
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
            _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }
        
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateStaffAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê  lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void DateOfBirth_MustLessThanCurrentDate()
        {
            var command = new UpdateStaffAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
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
            var command = new UpdateStaffAccountCommand
            {
                UserId = 10,
                Name = "Nguyên Lê",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147140.png",
                DateOfBirth = DateTime.Parse("2000-01-10"),
                Gender = "Female lorem lorem lorem"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }
    }
}

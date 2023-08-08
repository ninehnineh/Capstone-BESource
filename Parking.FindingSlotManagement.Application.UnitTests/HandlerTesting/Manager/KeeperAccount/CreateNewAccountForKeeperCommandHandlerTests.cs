using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.CreateNewAccountForKeeper;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.KeeperAccount
{
    public class CreateNewAccountForKeeperCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly CreateNewAccountForKeeperCommandHandler _handler;
        private readonly CreateNewAccountForKeeperCommandValidation _validator;
        public CreateNewAccountForKeeperCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _validator = new CreateNewAccountForKeeperCommandValidation();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new CreateNewAccountForKeeperCommandHandler(_userRepositoryMock.Object, _emailServiceMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidData_ShouldCreateNewAccountAndSendEmail()
        {
            // Arrange


            var command = new CreateNewAccountForKeeperCommand
            {
                Name = "Test",
                Avatar = "ima2.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Email = "Test@gmail.com",
                Gender = "Nam",
                ManagerId = 1,
                ParkingId = 1,
                Phone = "0111111111"
            };

            var existingManager = new User
            {
                UserId = 1,
                RoleId = 1 // Assuming RoleId 1 represents Manager role
                           // Add other properties of the existing manager
            };

            var existingParking = new Domain.Entities.Parking
            {
                ParkingId = 1,
                IsActive = true // Assuming the parking is active
                                // Add other properties of the existing parking
            };

            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingManager);
            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingParking);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Message.ShouldBe("Thành công");

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true), Times.Exactly(2)); // Once for email and once for phone
            _userRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Once);
            _emailServiceMock.Verify(service => service.SendMail(It.IsAny<EmailModel>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ExistingPhoneNumber_ShouldReturnBadRequest()
        {
            // Arrange

            var command = new CreateNewAccountForKeeperCommand
            {
                Name = "Test",
                Avatar = "ima2.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Email = "Test@gmail.com",
                Gender = "Nam",
                ManagerId = 1,
                ParkingId = 1,
                Phone = "0111111111"
            };

            var existingUser = new User
            {
                UserId = 1,
                Phone = "0111111111"
                // Add properties of the existing user with the same phone number
            };


            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true)).ReturnsAsync(existingUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Số điện thoại đã tồn tại. Vui lòng nhập số điện thoại khác!!!");

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
            _parkingRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
            _userRepositoryMock.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Never);
            _emailServiceMock.Verify(service => service.SendMail(It.IsAny<EmailModel>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ManagerNotFound_ShouldReturnBadRequest()
        {
            // Arrange


            var command = new CreateNewAccountForKeeperCommand
            {
                Name = "Test",
                Avatar = "ima2.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Email = "Test@gmail.com",
                Gender = "Nam",
                ManagerId = 1,
                ParkingId = 1,
                Phone = "0111111111"
            };

           

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản quản lý.");
        }
        [Fact]
        public async Task Handle_InactiveParking_ShouldReturnBadRequest()
        {


            var command = new CreateNewAccountForKeeperCommand
            {
                Name = "Test",
                Avatar = "ima2.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Email = "Test@gmail.com",
                Gender = "Nam",
                ManagerId = 1,
                ParkingId = 1,
                Phone = "0111111111"
            };

            var existingManager = new User
            {
                UserId = 1,
                RoleId = 1 // Assuming RoleId 1 represents Manager role
                           // Add other properties of the existing manager
            };

            var inactiveParking = new Domain.Entities.Parking
            {
                ParkingId = 1,
                IsActive = false // Assuming the parking is inactive
                                 // Add other properties of the inactive parking
            };


            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingManager);
            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(inactiveParking);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Bãi đã bị ban nên không thể thêm nhân viên vào bãi.");

            // Additional assertions if needed
            _userRepositoryMock.Verify(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true), Times.Exactly(2)); // Once for email and once for phone
            _userRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Never);
            _emailServiceMock.Verify(service => service.SendMail(It.IsAny<EmailModel>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {

            var command = new CreateNewAccountForKeeperCommand
            {
                Name = "Test",
                Avatar = "ima2.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                Email = "Test@gmail.com",
                Gender = "Nam",
                ManagerId = 1,
                ParkingId = 1,
                Phone = "0111111111"
            };


            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true)).Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
        public void DateOfBirth_ShouldNotBeEmpty()
        {
            var command = new CreateNewAccountForKeeperCommand
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
        public void DateOfBirth_ShouldNotBeNull()
        {
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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
            var command = new CreateNewAccountForKeeperCommand
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

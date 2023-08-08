using AutoMapper;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class CreateParkingPriceCommandHandlerTests
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly CreateParkingPriceCommandValidation _validator;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly CreateParkingPriceCommandHandler _handler;

        public CreateParkingPriceCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _handler = new CreateParkingPriceCommandHandler(_parkingPriceRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object);
            _validator = new CreateParkingPriceCommandValidation(_parkingPriceRepositoryMock.Object,
                _userRepositoryMock.Object, _trafficRepositoryMock.Object, _businessProfileRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenValidData()
        {
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "Test",
                TrafficId = 1,
                StartingTime = 1,
                IsExtrafee = true,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
            };

            var checkUserExist = new User { UserId = 1, RoleId = 1 };
            
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = 1, UserId = 1 };
            _businessProfileRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true)).ReturnsAsync(businessExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(checkTrafficExist);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _parkingPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingPrice>()), Times.Once);
        }

        [Fact]
        public async Task Should_Have_Error_When_ParkingPriceName_Is_LessThan_250()
        {
            // Arrange
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "testtesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt",
                TrafficId = 1
            };
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceName)
                .WithErrorMessage("Parking Price Name không được nhập quá 250 kí tự")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async Task Should_Have_Error_When_ParkingPriceName_Is_Null()
        {
            // Arrange
            var command = new CreateParkingPriceCommand { ManagerId = 1, ParkingPriceName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceName)
                .WithErrorMessage("Vui lòng nhập Parking Price Name.")
                .WithSeverity(Severity.Error);
        }

        [Fact]
        public async Task Should_Have_Error_When_TrafficId_Is_Null()
        {
            // Arrange
            var command = new CreateParkingPriceCommand { ManagerId = 1, ParkingPriceName = "test" };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TrafficId)
                .WithErrorMessage("Vui lòng nhập Traffic Id.")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async Task Should_Have_Error_When_TrafficId_Is_LessThan_0()
        {
            // Arrange
            var command = new CreateParkingPriceCommand { ManagerId = 1, ParkingPriceName = "test", TrafficId = -1 };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TrafficId)
                .WithErrorMessage("{TrafficId} phải lớn hơn 0")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async Task Should_Have_Error_When_TrafficId_Does_Not_Exist()
        {
            // Arrange
            var command = new CreateParkingPriceCommand { ManagerId = 1, ParkingPriceName = "Test12", TrafficId = 1 };

            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TrafficId)
                .WithErrorMessage("Phương tiện không tồn tại")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async void StartingTime_ShouldNotBeEmpty()
        {
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "Test",
                TrafficId = 1,
                StartingTime = null,
                IsExtrafee = true,
            };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(checkTrafficExist);

            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartingTime)
                .WithErrorMessage("Vui lòng nhập {Số tiếng khởi điểm}.")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async void StartingTime_ShouldNotBeNull()
        {
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "Test",
                TrafficId = 1,
                IsExtrafee = true,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
            };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(checkTrafficExist);
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartingTime)
                .WithErrorMessage("Vui lòng nhập {Số tiếng khởi điểm}.")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async void StartingTime_ShouldNotLessThan_0()
        {
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "Test",
                TrafficId = 1,
                StartingTime = -1,
                IsExtrafee = true,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
            };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(checkTrafficExist);
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartingTime)
                .WithErrorMessage("{Số tiếng khởi điểm} phải lớn hơn 0")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async void StartingTime_ShouldNotGreaterThan_24()
        {
            var command = new CreateParkingPriceCommand
            {
                ManagerId = 1,
                ParkingPriceName = "Test",
                TrafficId = 1,
                StartingTime = 25,
                IsExtrafee = true,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
            };
            var checkUserExist = new User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.ManagerId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(checkTrafficExist);
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartingTime)
                .WithErrorMessage("{Số tiếng khởi điểm} phải nhỏ hơn hoặc bằng 24")
                .WithSeverity(Severity.Error);
        }
    }
}

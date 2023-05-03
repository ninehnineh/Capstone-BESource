using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.CreateNewVehicleInfo;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfo.VehicleInfoManagement
{
    public class VehicleInfoCommandHandlerTests
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly VehicleInfoCommandHandler _handler;
        private readonly VehicleInfoCommandValidation _validator;
        public VehicleInfoCommandHandlerTests()
        {
            _vehicleInforRepositoryMock = new Mock<IVehicleInfoRepository>();
            _validator = new VehicleInfoCommandValidation();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new VehicleInfoCommandHandler(_vehicleInforRepositoryMock.Object, _accountRepositoryMock.Object, _trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 1
            };

            var checkUserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(checkTrafficExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _vehicleInforRepositoryMock.Verify(x => x.Insert(It.IsAny<VehicleInfor>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidUserId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 2,
                TrafficId = 1
            };

            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe(200);

            _vehicleInforRepositoryMock.Verify(x => x.Insert(It.IsAny<VehicleInfor>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidTrafficId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 10
            };
            var checkUserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync(checkUserExist);
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync((Traffic)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy phương tiện.");
            result.StatusCode.ShouldBe(200);

            _vehicleInforRepositoryMock.Verify(x => x.Insert(It.IsAny<VehicleInfor>()), Times.Never);
        }
        [Fact]
        public void LicensePlate_ShouldNotBeEmpty()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }
        [Fact]
        public void LicensePlate_ShouldNotBeNull()
        {
            var command = new VehicleInfoCommand
            {
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }
        [Fact]
        public void LicensePlate_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99777656545454545454545454545",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }

        [Fact]
        public void VehicleName_ShouldNotBeEmpty()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "",
                Color = "Nâu Vàng",
                UserId = 2,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }
        [Fact]
        public void VehicleName_ShouldNotBeNull()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                Color = "Nâu Vàng",
                UserId = 2,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }
        [Fact]
        public void VehicleName_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLvLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 2,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }

        [Fact]
        public void Color_ShouldNotBeEmpty()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "",
                UserId = 7,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
        [Fact]
        public void Color_ShouldNotBeNull()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                UserId = 7,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
        [Fact]
        public void Color_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu Vàngv",
                UserId = 7,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
    }
}

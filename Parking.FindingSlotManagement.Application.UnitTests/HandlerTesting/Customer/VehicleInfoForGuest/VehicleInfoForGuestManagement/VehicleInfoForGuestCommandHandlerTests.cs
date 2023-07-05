using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement
{
    public class VehicleInfoForGuestCommandHandlerTests 
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly VehicleInfoForGuestHandler _handler;
        private readonly VehicleInfoForGuestValidation _validator;
        public VehicleInfoForGuestCommandHandlerTests()
        {
            _vehicleInforRepositoryMock = new Mock<IVehicleInfoRepository>();
            _validator = new VehicleInfoForGuestValidation();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new VehicleInfoForGuestHandler(_vehicleInforRepositoryMock.Object, _trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 1
            };

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
        public async Task Handle_InvalidTrafficId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 10
            };
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
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }
        [Fact]
        public void LicensePlate_ShouldNotBeNull()
        {
            var command = new VehicleInfoForGuestCommand
            {
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }
        [Fact]
        public void LicensePlate_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99777656545454545454545454545",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 10
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }

        [Fact]
        public void VehicleName_ShouldNotBeEmpty()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "",
                Color = "Nâu Vàng",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }
        [Fact]
        public void VehicleName_ShouldNotBeNull()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                Color = "Nâu Vàng",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }
        [Fact]
        public void VehicleName_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLvLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hLLexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.VehicleName);
        }

        [Fact]
        public void Color_ShouldNotBeEmpty()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
        [Fact]
        public void Color_ShouldNotBeNull()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
        [Fact]
        public void Color_ShouldNotExceedMaximumLength()
        {
            var command = new VehicleInfoForGuestCommand
            {
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu VàngNâu Vàngv",
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Color);
        }
    }
}

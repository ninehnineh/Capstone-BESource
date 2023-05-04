using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.UpdateVehicleInfo;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfo.VehicleInfoManagement
{
    public class UpdateVehicleInfoCommandHandlerTests
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly UpdateVehicleInfoCommandHandler _handler;
        private readonly UpdateVehicleInfoCommandValidation _validator;
        public UpdateVehicleInfoCommandHandlerTests()
        {
            _vehicleInforRepositoryMock = new Mock<IVehicleInfoRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _validator = new UpdateVehicleInfoCommandValidation();
            _handler = new UpdateVehicleInfoCommandHandler(_vehicleInforRepositoryMock.Object, _accountRepositoryMock.Object, _trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateVehicleInfoCommandHandler_Should_Update_VehicleInfor_Successfully()
        {
            // Arrange
            var request = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldVehicleInfor = new VehicleInfor
            {
                VehicleInforId = 1,
                LicensePlate = "51G-678.89",
                VehicleName = "Mercedes G63",
                Color = "Black",
                UserId = 10,
                TrafficId = 1
            };

            _vehicleInforRepositoryMock.Setup(x => x.GetById(request.VehicleInforId))
                .ReturnsAsync(OldVehicleInfor);
            var checkUserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkUserExist);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(checkTrafficExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldVehicleInfor.LicensePlate.ShouldBe(request.LicensePlate);
            OldVehicleInfor.VehicleName.ShouldBe(request.VehicleName);
            OldVehicleInfor.Color.ShouldBe(request.Color);
            OldVehicleInfor.UserId.ShouldBe(request.UserId);
            OldVehicleInfor.TrafficId.ShouldBe(request.TrafficId);
            _vehicleInforRepositoryMock.Verify(x => x.Update(OldVehicleInfor), Times.Once);
        }
        [Fact]
        public async Task UpdateVehicleInfoCommandHandler_Should_Return_Not_Found_If_VehicleInfor_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 2000
            };
            var cancellationToken = new CancellationToken();
            _vehicleInforRepositoryMock.Setup(x => x.GetById(request.VehicleInforId))
                .ReturnsAsync((VehicleInfor)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy thông tin phương tiện.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _vehicleInforRepositoryMock.Verify(x => x.Update(It.IsAny<VehicleInfor>()), Times.Never);
        }
        [Fact]
        public async Task UpdateVehicleInfoCommandHandler_Should_Return_Not_Found_If_UserId_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 2,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldVehicleInfor = new VehicleInfor
            {
                VehicleInforId = 1,
                LicensePlate = "51G-678.89",
                VehicleName = "Mercedes G63",
                Color = "Black",
                UserId = 10,
                TrafficId = 1
            };
            _vehicleInforRepositoryMock.Setup(x => x.GetById(request.VehicleInforId))
               .ReturnsAsync(OldVehicleInfor);
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync((User)null);
            var checkTrafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(checkTrafficExist);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _vehicleInforRepositoryMock.Verify(x => x.Update(It.IsAny<VehicleInfor>()), Times.Never);
        }
        [Fact]
        public async Task UpdateVehicleInfoCommandHandler_Should_Return_Not_Found_If_TrafficId_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
                LicensePlate = "80A-919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                UserId = 7,
                TrafficId = 10
            };
            var cancellationToken = new CancellationToken();
            var OldVehicleInfor = new VehicleInfor
            {
                VehicleInforId = 1,
                LicensePlate = "51G-678.89",
                VehicleName = "Mercedes G63",
                Color = "Black",
                UserId = 10,
                TrafficId = 1
            };
            _vehicleInforRepositoryMock.Setup(x => x.GetById(request.VehicleInforId))
               .ReturnsAsync(OldVehicleInfor);
            var checkUserExist = new User { UserId = 7 };

            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkUserExist);
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync((Traffic)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy phương tiện.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _vehicleInforRepositoryMock.Verify(x => x.Update(It.IsAny<VehicleInfor>()), Times.Never);
        }
        [Fact]
        public void LicensePlate_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
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
        public void VehicleName_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
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
        public void Color_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateVehicleInfoCommand
            {
                VehicleInforId = 1,
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

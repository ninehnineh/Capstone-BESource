﻿using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.DeleteVehicleInfoForGuest;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement
{
    public class DeleteVehicleInfoForGuestHandlerTests
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepositoryMock;
        private readonly DeleteVehicleInforForGuestHandler _handler;
        public DeleteVehicleInfoForGuestHandlerTests()
        {
            _vehicleInforRepositoryMock = new Mock<IVehicleInfoRepository>();
            _handler = new DeleteVehicleInforForGuestHandler(_vehicleInforRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_DeleteVehicleInfor_When_Exist()
        {
            // Arrange
            var command = new DeleteVehicleInforForGuestCommand { VehicleInforId = 1 };
            var vehicleInfor = new VehicleInfor
            {
                VehicleInforId = command.VehicleInforId,
                LicensePlate = "80A - 919.99",
                VehicleName = "Lexus LS600hL",
                Color = "Nâu Vàng",
                TrafficId = 1

            };
            _vehicleInforRepositoryMock.Setup(x => x.GetById(command.VehicleInforId)).ReturnsAsync(vehicleInfor);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _vehicleInforRepositoryMock.Verify(x => x.Delete(vehicleInfor), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeleteVehicleInforForGuestCommand { VehicleInforId = 1 };
            _vehicleInforRepositoryMock.Setup(x => x.GetById(command.VehicleInforId)).ReturnsAsync((Domain.Entities.VehicleInfor)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy thông tin phương tiện.");

            _vehicleInforRepositoryMock.Verify(x => x.Delete(It.IsAny<VehicleInfor>()), Times.Never);
        }
    }
}

using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSlots
{
    public class CreateParkingSlotsCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly CreateParkingSlotsCommandHandler _handler;
        public CreateParkingSlotsCommandHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _handler = new CreateParkingSlotsCommandHandler(_mapperMock.Object, _parkingSlotRepositoryMock.Object, _timeSlotRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_SlotNameAlreadyExists_ShouldReturnBadRequest()
        {
            // Arrange

            var command = new CreateParkingSlotsCommand
            {
                Name = "Slot1", // Replace with an existing slot name
                FloorId = 1 // Replace with an existing floorId
                            // Add other properties as needed for testing
            };

            _parkingSlotRepositoryMock.Setup(repo => repo.isExists(It.IsAny<Models.ParkingSlot.ParkingSlotDTO>())).ReturnsAsync(true);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tên chỗ để xe đã tồn tại");
            result.Data.ShouldBe(default(int));
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockParkingSlotRepository = new Mock<IParkingSlotRepository>();
            var mockTimeSlotRepository = new Mock<ITimeSlotRepository>();

            var command = new CreateParkingSlotsCommand
            {
                Name = "New Slot", // Replace with a non-existing slot name
                FloorId = 1 // Replace with an existing floorId
                            // Add other properties as needed for testing
            };

            mockParkingSlotRepository.Setup(repo => repo.isExists(It.IsAny<Models.ParkingSlot.ParkingSlotDTO>())).Throws(new Exception("Simulated exception"));

            var handler = new CreateParkingSlotsCommandHandler(
                mockMapper.Object,
                mockParkingSlotRepository.Object,
                mockTimeSlotRepository.Object);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}

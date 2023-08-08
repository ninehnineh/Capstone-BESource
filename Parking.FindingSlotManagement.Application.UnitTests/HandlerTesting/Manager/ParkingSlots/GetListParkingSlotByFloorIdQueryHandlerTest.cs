using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetListParkingSlotByFloorId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSlots
{
    public class GetListParkingSlotByFloorIdQueryHandlerTest
    {
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly GetListParkingSlotByFloorIdQueryHandler _handler;
        public GetListParkingSlotByFloorIdQueryHandlerTest()
        {
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _handler = new GetListParkingSlotByFloorIdQueryHandler(_parkingSlotRepositoryMock.Object, _floorRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidFloorId_ShouldReturnListOfParkingSlots()
        {
            // Arrange

            var query = new GetListParkingSlotByFloorIdQuery
            {
                FloorId = 1 // Replace with an existing floor Id
            };

            var existingFloor = new Floor
            {
                FloorId = 1,
                // Add other properties of the existing floor
            };

            var existingParkingSlots = new List<ParkingSlot>
            {
                new ParkingSlot
                {
                    ParkingSlotId = 1,
                    FloorId = 1
                },
                new ParkingSlot
                {
                    ParkingSlotId = 2,
                    FloorId = 1
                }
                // Add more parking slots if needed
            };

            _floorRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingFloor);
            _parkingSlotRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>())).ReturnsAsync(existingParkingSlots);


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeEmpty();
            result.Count.ShouldBe(existingParkingSlots.Count);

            // Additional assertions if needed
            _floorRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidFloorId_ShouldReturnBadRequest()
        {
            // Arrange

            var query = new GetListParkingSlotByFloorIdQuery
            {
                FloorId = 1 // Replace with a non-existing floor Id
            };

            _floorRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Floor)null);


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tầng.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);

            // Additional assertions if needed
            _floorRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ValidFloorId_NoParkingSlots_ShouldReturnEmptyList()
        {
            // Arrange

            var query = new GetListParkingSlotByFloorIdQuery
            {
                FloorId = 1 // Replace with an existing floor Id
            };

            var existingFloor = new Floor
            {
                FloorId = 1,
                // Add other properties of the existing floor
            };

            _floorRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingFloor);
            _parkingSlotRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>())).ReturnsAsync((List<ParkingSlot>)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy slot.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);

            // Additional assertions if needed
            _floorRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange

            var query = new GetListParkingSlotByFloorIdQuery
            {
                FloorId = 1 // Replace with an existing floor Id
            };

            var existingFloor = new Floor
            {
                FloorId = 1,
                // Add other properties of the existing floor
            };

            _floorRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingFloor);
            _parkingSlotRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>())).Throws(new Exception("Simulated exception"));


            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.

            // Additional assertions if needed
            _floorRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<ParkingSlot, bool>>>()), Times.Once);
        }
    }
}

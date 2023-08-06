using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloorByParkingId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Floors.FloorManagement
{
    public class GetListFloorByParkingIdQueryHandlerTest
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetListFloorByParkingIdQueryHandler _handler;
        public GetListFloorByParkingIdQueryHandlerTest()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetListFloorByParkingIdQueryHandler(_floorRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidParkingId_ShouldReturnListOfFloors()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parking ID


            var floorList = new List<Floor>
            {
                new Floor { FloorId = 1, ParkingId = 1},
                new Floor { FloorId = 2, ParkingId =  1},
                new Floor { FloorId = 3, ParkingId = 1 }
            };

            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Parking { ParkingId = parkingId });

            _floorRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Floor, bool>>>()))
                .ReturnsAsync(floorList);

            var query = new GetListFloorByParkingIdQuery { ParkingId = parkingId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(floorList.Count);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ShouldReturnNotFoundResponse()
        {
            // Arrange
            int parkingId = 999; // Replace with an invalid parking ID


            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Domain.Entities.Parking)null); // Return null for an invalid parking ID

            var query = new GetListFloorByParkingIdQuery { ParkingId = parkingId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
        }
        [Fact]
        public async Task Handle_NoFloorsFound_ShouldReturnEmptyResponse()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parking ID

            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Parking { ParkingId = parkingId, /* Fill with relevant data */ });

            _floorRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Floor, bool>>>()))
                .ReturnsAsync((List<Floor>)null); // Return null for no floors found

            var query = new GetListFloorByParkingIdQuery { ParkingId = parkingId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parking ID


            _parkingRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Throws(new Exception("Simulated exception"));

            var query = new GetListFloorByParkingIdQuery { ParkingId = parkingId };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}

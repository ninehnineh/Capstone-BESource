using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.ParkingSlot
{
    public class GetAvailableSlotByFloorIdQueryHandlerTests
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetAvailableSlotByFloorIdQueryHandler _queryHandler;

        public GetAvailableSlotByFloorIdQueryHandlerTests()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); // Replace this with your actual mapping profile.
            }).CreateMapper();

            _queryHandler = new GetAvailableSlotByFloorIdQueryHandler(
                _floorRepositoryMock.Object,
                _parkingSlotRepositoryMock.Object,
                _timeSlotRepositoryMock.Object
            );
        }
        [Fact]
        public async Task Handle_FloorNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var floorId = 123;
            var request = new GetAvailableSlotByFloorIdQuery { FloorId = floorId };

            _floorRepositoryMock.Setup(x => x.GetById(floorId)).ReturnsAsync((Floor)null);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tầng.");
            result.Data.ShouldBeNull();
            _floorRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ParkingSlot, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingSlot, object>>>>(), null, true), Times.Never);
            _timeSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<TimeSlot, bool>>>(), It.IsAny<List<Expression<Func<TimeSlot, object>>>>(), null, true), Times.Never);
        }
        [Fact]
        public async Task Handle_NoTimeSlotsFound_ReturnsErrorResponse()
        {
            // Arrange
            var floorId = 123;
            var request = new GetAvailableSlotByFloorIdQuery { FloorId = floorId };
            var floor = new Floor { FloorId = floorId };

            _floorRepositoryMock.Setup(x => x.GetById(floorId)).ReturnsAsync(floor);
            _timeSlotRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<TimeSlot, bool>>>(), It.IsAny<List<Expression<Func<TimeSlot, object>>>>(), null, true)).ReturnsAsync(new List<TimeSlot>());

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Chưa có timeSlot");
            result.Data.ShouldBeNull();
            _floorRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _timeSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<TimeSlot, bool>>>(), It.IsAny<List<Expression<Func<TimeSlot, object>>>>(), null, true), Times.Once);
            _parkingSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ParkingSlot, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingSlot, object>>>>(), null, true), Times.Never);
        }
        /*[Fact]
        public async Task Handle_ParkingSlotsFound_ReturnsValidResponse()
        {
            // Arrange
            var floorId = 123;
            var request = new GetAvailableSlotByFloorIdQuery { FloorId = floorId };
            var floor = new Floor { FloorId = floorId };
            var timeSlots = new List<TimeSlot>
            {
                new TimeSlot { TimeSlotId = 1, ParkingSlotId = 1 },
                new TimeSlot { TimeSlotId = 2, ParkingSlotId = 2 }
            };
            var parkingSlots = new List<Domain.Entities.ParkingSlot>
            {
                new Domain.Entities.ParkingSlot { ParkingSlotId = 1 },
                new Domain.Entities.ParkingSlot { ParkingSlotId = 2 }
            };
            var expectedResponse = new ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>
            {
                Data = new List<GetAvailableSlotByFloorIdResponse>
                {
                    new GetAvailableSlotByFloorIdResponse { ParkingSlotDto = new ParkingSlotDto { ParkingSlotId = 1, FloorId = 1}, IsBooked = 0 },
                    new GetAvailableSlotByFloorIdResponse { ParkingSlotDto = new ParkingSlotDto { ParkingSlotId = 2, FloorId = 1}, IsBooked = 1 }
                },
                Success = true,
                StatusCode = 200,
                Message = "Thành công",
                Count = 2
            };

            _floorRepositoryMock.Setup(x => x.GetById(floorId)).ReturnsAsync(floor);
            _timeSlotRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<TimeSlot, bool>>>(), It.IsAny<List<Expression<Func<TimeSlot, object>>>>(), null, true)).ReturnsAsync(timeSlots);
            _parkingSlotRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ParkingSlot, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingSlot, object>>>>(), null, true)).ReturnsAsync(parkingSlots);
            Mock<IMapper> x = new Mock<IMapper>();
            x.Setup(x => x.Map<IEnumerable<GetAvailableSlotByFloorIdResponse>>(It.IsAny<List<ParkingSlotDto>>())).Returns(expectedResponse.Data);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Data.ShouldBe(expectedResponse.Data);
            result.Count.ShouldBe(2);
            _floorRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _timeSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<TimeSlot, bool>>>(), It.IsAny<List<Expression<Func<TimeSlot, object>>>>(), null, true), Times.Once);
            _parkingSlotRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ParkingSlot, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingSlot, object>>>>(), null, true), Times.Exactly(3));
            x.Verify(x => x.Map<IEnumerable<GetAvailableSlotByFloorIdResponse>>(It.IsAny<List<ParkingSlotDto>>()), Times.Once);
        }*/

    }
}

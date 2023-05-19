/*using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Timeline.TimelineManagement
{
    public class CreateNewTimelineCommandHandlerTests
    {
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly CreateNewTimelineCommandHandler _handler;
        public CreateNewTimelineCommandHandlerTests()
        {
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _handler = new CreateNewTimelineCommandHandler(_timelineRepositoryMock.Object, _trafficRepositoryMock.Object, _parkingPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = DateTime.Parse("2023-05-17T06:00:00"),
                EndTime = DateTime.Parse("2023-05-17T18:00:00"),
                StartingTime = 2,
                IsExtrafee = true,
                ExtraFee = 20000,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
                TrafficId = 2,
                ParkingPriceId = 1
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 2 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _timelineRepositoryMock.Verify(x => x.Insert(It.IsAny<TimeLine>()), Times.Once);
        }
        [Fact]
        public async Task Handle_WithNonExistingTraffic_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = DateTime.Parse("2023-05-17T06:00:00"),
                EndTime = DateTime.Parse("2023-05-17T18:00:00"),
                StartingTime = 2,
                IsExtrafee = true,
                ExtraFee = 20000,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
                TrafficId = 2,
                ParkingPriceId = 1
            };

            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync((Traffic)null);
            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Không tìm thấy phương tiện.");
        }
        [Fact]
        public async Task Handle_WithNonExistingParkingPrice_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = DateTime.Parse("2023-05-17T06:00:00"),
                EndTime = DateTime.Parse("2023-05-17T18:00:00"),
                StartingTime = 2,
                IsExtrafee = true,
                ExtraFee = 20000,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
                TrafficId = 2,
                ParkingPriceId = 1
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 2 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync((Domain.Entities.ParkingPrice)null);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Không tìm thấy gói.");
        }
        [Fact]
        public async Task Handle_WithExistingTimelinesAndInvalidStartTime_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartingTime = 2,
                IsExtrafee = true,
                ExtraFee = 20000,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
                TrafficId = 2,
                ParkingPriceId = 1,
                StartTime = DateTime.Parse("2023-05-07T06:00:00"),
                EndTime = DateTime.Parse("2023-05-07T18:00:00"), // Set a valid end time
                                                                 // Set other properties as needed
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 2 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);

            var existingTimelines = new List<TimeLine>
            {
                new TimeLine
                {
                    StartTime = DateTime.UtcNow.Date.AddHours(8), // Start time of the existing timeline
                    EndTime = DateTime.UtcNow.Date.AddHours(10), // End time of the existing timeline
                    IsActive = true
                }
            };

            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(
                    It.IsAny<Expression<Func<TimeLine, bool>>>(),
                    null,
                    x => x.TimeLineId,
                    true))
                .ReturnsAsync(existingTimelines); // Return the existing timelines

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.");

            timelineRepositoryMock.Verify(x => x.Insert(It.IsAny<TimeLine>()), Times.Never); // Insert method should not be called
        }
        [Fact]
        public async Task Handle_WithExistingTimelinesAndInvalidEndTime_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartingTime = 2,
                IsExtrafee = true,
                ExtraFee = 20000,
                ExtraTimeStep = 2,
                HasPenaltyPrice = true,
                PenaltyPrice = 30000,
                PenaltyPriceStepTime = 2,
                TrafficId = 2,
                ParkingPriceId = 1,
                StartTime = DateTime.Parse("2023-05-17T06:00:00"),
                EndTime = DateTime.Parse("2023-05-20T18:00:00"), // Set a valid end time
                                                                 // Set other properties as needed
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 2 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);

            var existingTimelines = new List<TimeLine>
            {
                new TimeLine
                {
                    StartTime = DateTime.UtcNow.Date.AddHours(8), // Start time of the existing timeline
                    EndTime = DateTime.UtcNow.Date.AddHours(10), // End time of the existing timeline
                    IsActive = true
                }
            };

            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(
                    It.IsAny<Expression<Func<TimeLine, bool>>>(),
                    null,
                    x => x.TimeLineId,
                    true))
                .ReturnsAsync(existingTimelines); // Return the existing timelines

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.");

            timelineRepositoryMock.Verify(x => x.Insert(It.IsAny<TimeLine>()), Times.Never); // Insert method should not be called
        }
    }
}
*/
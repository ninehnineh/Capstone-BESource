using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.DisableOrEnableTimeline;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Timeline.TimelineManagement
{
    public class DisableOrEnableTimelineCommandHandlerTest
    {
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly DisableOrEnableTimelineCommandHandler _handler;
        public DisableOrEnableTimelineCommandHandlerTest()
        {
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _handler = new DisableOrEnableTimelineCommandHandler(_timelineRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableTimelineCommand
            {
                TimeLineId = 1
            };

            var timelineToDelete = new TimeLine
            {
                TimeLineId = 1,
                IsActive = true,
            };

            _timelineRepositoryMock.Setup(x => x.GetById(request.TimeLineId))
                .ReturnsAsync(timelineToDelete);

            _timelineRepositoryMock.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _timelineRepositoryMock.Verify(x => x.GetById(request.TimeLineId), Times.Once);
            _timelineRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Floor_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableTimelineCommand
            {
                TimeLineId = 999999
            };

            _timelineRepositoryMock.Setup(x => x.GetById(request.TimeLineId))
                .ReturnsAsync((TimeLine)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy khung giờ.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _timelineRepositoryMock.Verify(x => x.GetById(request.TimeLineId), Times.Once);
            _timelineRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableTimelineCommand
            {
                TimeLineId = 1
            };

            _timelineRepositoryMock.Setup(x => x.GetById(request.TimeLineId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _timelineRepositoryMock.Verify(x => x.GetById(request.TimeLineId), Times.Once);
            _timelineRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}

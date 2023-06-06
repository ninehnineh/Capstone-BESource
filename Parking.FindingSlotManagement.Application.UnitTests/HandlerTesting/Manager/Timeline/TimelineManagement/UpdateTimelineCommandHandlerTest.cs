using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.UpdateTimeline;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Timeline.TimelineManagement
{
    public class UpdateTimelineCommandHandlerTest
    {
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly UpdateTimelineCommandHandler _handler;
        private readonly UpdateTimelineCommandValidation _validator;
        public UpdateTimelineCommandHandlerTest()
        {
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _validator = new UpdateTimelineCommandValidation();
            _handler = new UpdateTimelineCommandHandler(_timelineRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateTimelineCommandHandler_Should_Update_Timeline_Successfully()
        {
            // Arrange
            var request = new UpdateTimelineCommand
            {
                TimeLineId = 1,
                Name = "Khung modify",
                Price = 30000
            };
            var cancellationToken = new CancellationToken();
            var OldTimeLine = new TimeLine
            {
                TimeLineId = 1,
                Name = "Tầng 1",
                IsActive = true,
                Price = 20000
            };
            _timelineRepositoryMock.Setup(x => x.GetById(request.TimeLineId))
                .ReturnsAsync(OldTimeLine);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldTimeLine.Name.ShouldBe(request.Name);
            _timelineRepositoryMock.Verify(x => x.Update(OldTimeLine), Times.Once);
        }
        [Fact]
        public async Task UpdateTimelineCommandHandler_Should_Return_Not_Found_If_Timeline_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateTimelineCommand
            {
                TimeLineId = 2000
            };
            var cancellationToken = new CancellationToken();
            _timelineRepositoryMock.Setup(x => x.GetById(request.TimeLineId))
                .ReturnsAsync((TimeLine)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy khung giờ.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _timelineRepositoryMock.Verify(x => x.Update(It.IsAny<TimeLine>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateTimelineCommand
            {
                TimeLineId = 1,
                Name = "Khung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodify",
                Price = 30000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Description_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateTimelineCommand
            {
                TimeLineId = 1,
                Description = "Khung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodifyKhung hung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifyhung modifymodify",
                Price = 30000
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Price_ShouldNotLessThan_0()
        {
            var command = new UpdateTimelineCommand
            {
                TimeLineId = 1,
                Name = "Khung hung modify",
                Price = -1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        
    }
}

using FluentValidation.TestHelper;
using Moq;
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
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly CreateNewTimelineCommandHandler _handler;
        private readonly CreateNewTimelineCommandValidation _validator;
        public CreateNewTimelineCommandHandlerTests()
        {
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _validator = new CreateNewTimelineCommandValidation();
            _handler = new CreateNewTimelineCommandHandler(_timelineRepositoryMock.Object, _parkingPriceRepositoryMock.Object);
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
                StartTime = "06:00:00",
                EndTime = "18:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

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
        public async Task Handle_WithNonExistingParkingPrice_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "06:00:00",
                EndTime = "18:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };


            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)!)
                .ReturnsAsync((Domain.Entities.ParkingPrice)null!);

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
        public async Task Handle_With_Adding_New_Timeline_But_In_The_Package_The_List_Of_Timeline_Have_24h_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "06:00:00",
                EndTime = "08:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };


            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);

            var lstTimeLine = new List<TimeLine>
            {
                new TimeLine { TimeLineId = 1, ParkingPriceId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00")},
                new TimeLine { TimeLineId = 2, ParkingPriceId = 1, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00")},
            };
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, x => x.TimeLineId, true)).ReturnsAsync(lstTimeLine);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(400);
            response.Success.ShouldBeFalse();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Không thể tạo mới khung giờ. Do các khung giờ trong gói đã đủ 24 tiếng. Bạn chỉ có thể tạo mới khi xóa các gói trước đó hoặc tạo mới 1 gói khác sau đó bạn có thể tạo mới khung giờ của gói đó.");
        }
        [Fact]
        public async Task Handle_The_EndTime_Must_Greater_Than_StartTime_For_1h_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "04:00:00",
                EndTime = "04:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };


            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);

            var lstTimeLine = new List<TimeLine>
            {
                new TimeLine { TimeLineId = 2, ParkingPriceId = 1, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("04:00:00")},
                new TimeLine { TimeLineId = 1, ParkingPriceId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00")},

            };
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, x => x.TimeLineId, true)).ReturnsAsync(lstTimeLine);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(400);
            response.Success.ShouldBeFalse();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Giờ kết thúc phải lớn hơn giờ bắt đầu 1 tiếng.");
        }
        [Fact]
        public async Task Handle_Invalid_Timeline_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };


            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);

            var lstTimeLine = new List<TimeLine>
            {
                new TimeLine { TimeLineId = 2, ParkingPriceId = 1, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("04:00:00")},
                new TimeLine { TimeLineId = 1, ParkingPriceId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00")},

            };
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, x => x.TimeLineId, true)).ReturnsAsync(lstTimeLine);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(400);
            response.Success.ShouldBeFalse();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Gói không hợp lệ.");
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewTimelineCommand
            {
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngàyGói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Description_ShouldNotBeEmpty()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotBeNull()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = 5000,
                Description = "Gói giữ xe máy vào ban ngàyGói giữ xe máy vào ban ngàyGói giữ xe máy vào ban ngàyGói giữ xe máy vào ban ngàyGói giữ xe máy vào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngàyvào ban ngàyGói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Price_ShouldNotBeEmpty()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = null,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Price_ShouldNotBeNull()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Price_ShouldNotLessThan_0()
        {
            var command = new CreateNewTimelineCommand
            {
                Name = "Gói giữ xe ban ngày",
                Price = -1,
                Description = "Gói giữ xe máy vào ban ngày",
                StartTime = "02:00:00",
                EndTime = "05:00:00",
                ExtraFee = 20000,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        
    }
}

using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement
{
    public class CreateParkingHasPriceCommandHandlerTest
    {
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly CreateParkingHasPriceCommandHandler _handler;
        private readonly CreateParkingHasPriceCommandValidation _validator;
        public CreateParkingHasPriceCommandHandlerTest()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _validator = new CreateParkingHasPriceCommandValidation();
            _handler = new CreateParkingHasPriceCommandHandler(_parkingHasPriceRepositoryMock.Object, _parkingRepositoryMock.Object, _parkingPriceRepositoryMock.Object, _timelineRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 0 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = true, TrafficId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            var lstParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>();
            _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, null, null, true)).ReturnsAsync(lstParkingHasPrice);

            var lstTimeLine = new List<TimeLine>
            {
                new TimeLine { TimeLineId = 1, ParkingPriceId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00")},
                new TimeLine { TimeLineId = 2, ParkingPriceId = 1, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00")},
            };
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, null, true)).ReturnsAsync(lstTimeLine);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            _parkingRepositoryMock.Setup(x => x.GetById(command.ParkingId)).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            result.StatusCode.ShouldBe(200);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidParkingPriceId_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync((Domain.Entities.ParkingPrice)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy gói.");
            result.StatusCode.ShouldBe(200);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidParkingPrice_IsActive_Is_False_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = false };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Gói không khả dụng.");
            result.StatusCode.ShouldBe(400);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        /*[Fact]
        public async Task Handle_InvalidParking_IsActive_Is_False_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50, IsActive = false };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = true };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Bãi giữ xe không khả dụng.");
            result.StatusCode.ShouldBe(400);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }*/
        [Fact]
        public async Task Handle_InvalidParkingHasPrice_Because_It_Already_Has_Package_Applying_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 0 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = true, TrafficId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            var lstParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>()
            {
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = 1, ParkingPriceId = 2}
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, null, null, true)).ReturnsAsync(lstParkingHasPrice);
            var parkingPriceExistVer2 = new Domain.Entities.ParkingPrice { ParkingPriceId = 2, IsActive = true, TrafficId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(lstParkingHasPrice.FirstOrDefault().ParkingPriceId)).ReturnsAsync(parkingPriceExistVer2);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Hiện tại đã có gói áp dụng, không thể áp dụng gói này. Nếu muốn sử dụng gói này thì hãy xóa gói đã áp dụng sau đó thêm áp dụng gói này.");
            result.StatusCode.ShouldBe(400);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidParkingHasPrice_Because_It_Already_Has_Package_Applying_Ver_WholeDay_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = true, IsWholeDay = true, TrafficId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            var lstParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>()
            {
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = 1, ParkingPriceId = 2}
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, null, null, true)).ReturnsAsync(lstParkingHasPrice);
            var parkingPriceExistVer2 = new Domain.Entities.ParkingPrice { ParkingPriceId = 2, IsActive = true, IsWholeDay = true };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(lstParkingHasPrice.FirstOrDefault().ParkingPriceId)).ReturnsAsync(parkingPriceExistVer2);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Hiện tại đã có gói áp dụng, không thể áp dụng gói này. Nếu muốn sử dụng gói này thì hãy xóa gói đã áp dụng sau đó thêm áp dụng gói này.");
            result.StatusCode.ShouldBe(400);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }

        [Fact]
        public async Task Handle_The_Package_Does_Not_Have_Timeline_ReturnsErrorResponse()
        {
            // Arrange
            var request = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = 1
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1, IsActive = true };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(parkingPriceExist);

            var lstParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>();
            _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, null, null, true)).ReturnsAsync(lstParkingHasPrice);

            var lstTimeLine = new List<TimeLine>();
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, null, true)).ReturnsAsync(lstTimeLine);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Gói chưa có khung giờ, vui lòng tạo mới khung trước khi áp dụng gói vào bãi giữ xe.");
            result.StatusCode.ShouldBe(400);

            _parkingHasPriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public void ParkingId_ShouldNotBeEmpty()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = null,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingId_ShouldNotBeNull()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingId_ShouldNotLessThan_0()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = -1,
                ParkingPriceId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingPriceId_ShouldNotBeEmpty()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceId);
        }
        [Fact]
        public void ParkingPriceId_ShouldNotBeNull()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceId);
        }
        [Fact]
        public void ParkingPriceId_ShouldNotLessThan_0()
        {
            var command = new CreateParkingHasPriceCommand
            {
                ParkingId = 1,
                ParkingPriceId = -1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceId);
        }
    }
}

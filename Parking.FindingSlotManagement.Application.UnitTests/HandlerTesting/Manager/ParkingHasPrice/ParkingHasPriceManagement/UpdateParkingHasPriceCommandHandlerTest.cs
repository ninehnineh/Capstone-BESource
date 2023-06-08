using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.UpdateParkingHasPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement
{
    public class UpdateParkingHasPriceCommandHandlerTest
    {
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly UpdateParkingHasPriceCommandHandler _handler;
        public UpdateParkingHasPriceCommandHandlerTest()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _handler = new UpdateParkingHasPriceCommandHandler(_parkingHasPriceRepositoryMock.Object, _parkingPriceRepositoryMock.Object, _parkingRepositoryMock.Object, _timelineRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Should_Update_ParkingHasPrice_Successfully()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            var OldParkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 1,
                ParkingId = 1
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync(OldParkingHasPrice);

            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(OldParkingHasPrice.ParkingId!)).ReturnsAsync(checkParkingExist);

            var checkParkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = request.ParkingPriceId, IsActive = true };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(checkParkingPriceExist);

            var lstTimeLine = new List<TimeLine>
            {
                new TimeLine { TimeLineId = 1, ParkingPriceId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00")},
                new TimeLine { TimeLineId = 2, ParkingPriceId = 1, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00")},
            };
            _timelineRepositoryMock.Setup(x => x.GetAllItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId && x.IsActive == true, null, null, true)).ReturnsAsync(lstTimeLine);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldParkingHasPrice.ParkingPriceId.ShouldBe(request.ParkingPriceId);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(OldParkingHasPrice), Times.Once);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Should_Return_Not_Found_If_ParkingHasPrice_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync((Domain.Entities.ParkingHasPrice)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tồn tại");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Should_Return_Not_Found_If_ParkingPrice_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            var OldParkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 1,
                ParkingId = 1
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync(OldParkingHasPrice);

            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(OldParkingHasPrice.ParkingId!)).ReturnsAsync(checkParkingExist);

            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync((Domain.Entities.ParkingPrice)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy gói.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Should_Return_Not_Found_If_ParkingPrice_Has_IsActive_Equals_False()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            var OldParkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 1,
                ParkingId = 1
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync(OldParkingHasPrice);

            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(OldParkingHasPrice.ParkingId!)).ReturnsAsync(checkParkingExist);

            var checkParkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = request.ParkingPriceId, IsActive = false };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(checkParkingPriceExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Gói không khả dụng.");
            response.StatusCode.ShouldBe(400);
            response.Count.ShouldBe(0);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Handle_The_Parking_Does_Not_Support_Car_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            var OldParkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 1,
                ParkingId = 1
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync(OldParkingHasPrice);

            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 0, MotoSpot = 50 };
            _parkingRepositoryMock.Setup(x => x.GetById(OldParkingHasPrice.ParkingId!)).ReturnsAsync(checkParkingExist);

            var checkParkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = request.ParkingPriceId, IsActive = true, TrafficId = 2 };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(checkParkingPriceExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Bãi giữ xe không hổ trợ xe hơi nên áp dụng gói không phù hợp.");
            response.StatusCode.ShouldBe(400);
            response.Count.ShouldBe(0);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingHasPriceCommandHandler_Handle_The_Parking_Does_Not_Support_Moto_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateParkingHasPriceCommand
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 2,
            };
            var cancellationToken = new CancellationToken();
            var OldParkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
                ParkingHasPriceId = 1,
                ParkingPriceId = 1,
                ParkingId = 1
            };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(request.ParkingHasPriceId))
                .ReturnsAsync(OldParkingHasPrice);

            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 1, CarSpot = 50, MotoSpot = 0 };
            _parkingRepositoryMock.Setup(x => x.GetById(OldParkingHasPrice.ParkingId!)).ReturnsAsync(checkParkingExist);

            var checkParkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = request.ParkingPriceId, IsActive = true, TrafficId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, true)).ReturnsAsync(checkParkingPriceExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Bãi giữ xe không hổ trợ xe mô tô nên áp dụng gói không phù hợp.");
            response.StatusCode.ShouldBe(400);
            response.Count.ShouldBe(0);
            _parkingHasPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingHasPrice>()), Times.Never);
        }
    }
}

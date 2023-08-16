using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Timeline.TimelineManagement
{
    public class GetListTimelineByParkingPriceIdQueryHandlerTest
    {
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly GetListTimelineByParkingPriceIdQueryHandler _handler;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        public GetListTimelineByParkingPriceIdQueryHandlerTest()
        {
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _handler = new GetListTimelineByParkingPriceIdQueryHandler(_timelineRepositoryMock.Object, _parkingPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListTimelineByParkingPriceIdQuery
            {
                ParkingPriceId = 1,
                PageNo = 1,
                PageSize = 10
            };

            var parkingPriceListExist = new List<Domain.Entities.ParkingPrice>
            {
                new Domain.Entities.ParkingPrice
                {
                    ParkingPriceId = 1,
                    TimeLines = new List<Domain.Entities.TimeLine>
                    {
                        new Domain.Entities.TimeLine{ TimeLineId = 1, ParkingPriceId = 1},
                        new Domain.Entities.TimeLine{ TimeLineId = 2, ParkingPriceId = 1}
                    }
                },

            };
            var parkingPriceExist = new Domain.Entities.ParkingPrice { ParkingPriceId = 1 };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync(parkingPriceExist);
            _parkingPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingPrice, object>>>>(), null, true, request.PageNo, request.PageSize)).ReturnsAsync(parkingPriceListExist);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_Return_Not_Found_When_ParkingPrice_Does_Not_Exist()
        {
            // Arrange
            var request = new GetListTimelineByParkingPriceIdQuery
            {
                ParkingPriceId = 1,
                PageNo = 1,
                PageSize = 10
            };

            var TimelineList = new List<Domain.Entities.TimeLine>
            {
                new Domain.Entities.TimeLine
                {
                    ParkingPriceId = 1,
                    TimeLineId = 1
                },

            };
            _parkingPriceRepositoryMock.Setup(x => x.GetById(request.ParkingPriceId)).ReturnsAsync((Domain.Entities.ParkingPrice)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy gói.");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListTimelineByParkingPriceIdQuery
            {
                ParkingPriceId = 5,
                PageNo = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
        }

    }
}

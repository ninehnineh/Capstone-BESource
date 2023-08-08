using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingWithIntial;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.ApproveParking
{
    public class GetApproveParkingWithIntialQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly GetApproveParkingWithIntialQueryHandler _queryHandler;

        public GetApproveParkingWithIntialQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetApproveParkingWithIntialQueryHandler(_parkingRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ParkingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetApproveParkingWithIntialQuery { ParkingId = 123 };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true)).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            _parkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true), Times.Once);
            _mapperMock.Verify(x => x.Map<GetApproveParkingWithIntialResponse>(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ParkingWithNoNewApproveParking_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetApproveParkingWithIntialQuery { ParkingId = 123 };
            var parking = new Domain.Entities.Parking { ParkingId = 123, ApproveParkings = new List<Domain.Entities.ApproveParking>() };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true)).ReturnsAsync(parking);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy đơn tạo mới.");
            _parkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true), Times.Once);
            _mapperMock.Verify(x => x.Map<GetApproveParkingWithIntialResponse>(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ParkingWithNewApproveParking_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetApproveParkingWithIntialQuery { ParkingId = 123 };
            var parking = new Domain.Entities.Parking { ParkingId = 123, ApproveParkings = new List<Domain.Entities.ApproveParking> { new Domain.Entities.ApproveParking { ApproveParkingId = 1, Status = ApproveParkingStatus.Tạo_mới.ToString() } } };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true)).ReturnsAsync(parking);
            _mapperMock.Setup(x => x.Map<GetApproveParkingWithIntialResponse>(It.IsAny<Domain.Entities.ApproveParking>())).Returns(new GetApproveParkingWithIntialResponse());

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            _parkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true), Times.Once);

        }
    }
}

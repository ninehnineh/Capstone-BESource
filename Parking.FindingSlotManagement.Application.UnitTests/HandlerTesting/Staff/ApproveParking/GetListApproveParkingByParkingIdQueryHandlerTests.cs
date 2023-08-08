using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId;
using Parking.FindingSlotManagement.Application.Mapping;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.ApproveParking
{
    public class GetListApproveParkingByParkingIdQueryHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly GetListApproveParkingByParkingIdQueryHandler _queryHandler;

        public GetListApproveParkingByParkingIdQueryHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _queryHandler = new GetListApproveParkingByParkingIdQueryHandler(_approveParkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_NoApproveParking_ReturnsEmptyListResponse()
        {
            // Arrange
            var request = new GetListApproveParkingByParkingIdQuery { ParkingId = 123 };
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true)).ReturnsAsync(new List<Domain.Entities.ApproveParking>());

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Danh sách trống.");
            _approveParkingRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true), Times.Once);
        }
        [Fact]
        public async Task Handle_ApproveParkingExist_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetListApproveParkingByParkingIdQuery { ParkingId = 123 };
            var approveParking1 = new Domain.Entities.ApproveParking { ApproveParkingId = 1, Status = "Tạo_mới" };
            var approveParking2 = new Domain.Entities.ApproveParking { ApproveParkingId = 2, Status = "Đã_duyệt" };
            var approveParkingList = new List<Domain.Entities.ApproveParking> { approveParking1, approveParking2 };
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true)).ReturnsAsync(approveParkingList);


            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Data.ShouldContain(x => x.ApproveParkingId == 1 && x.Status == "Tạo_mới");
            result.Data.ShouldContain(x => x.ApproveParkingId == 2 && x.Status == "Đã_duyệt");
            _approveParkingRepositoryMock.Verify(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true), Times.Once);
        }
    }
}

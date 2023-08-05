using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.ApproveParking
{
    public class GetApproveParkingByIdQueryHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly GetApproveParkingByIdQueryHandler _queryHandler;

        public GetApproveParkingByIdQueryHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetApproveParkingByIdQueryHandler(_approveParkingRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new GetApproveParkingByIdQuery { ApproveParkingId = 123 };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), true)).ReturnsAsync((Domain.Entities.ApproveParking)null);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");
            _approveParkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), true), Times.Once);
            _mapperMock.Verify(x => x.Map<List<ImagesOfRequestApprove>>(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestFound_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetApproveParkingByIdQuery { ApproveParkingId = 123 };
            var approveParking = new Domain.Entities.ApproveParking { ApproveParkingId = 123, StaffId = 1, User = new User { UserId = 1, Name = "Ga"}, Status = "Tạo_mới", CreatedDate = DateTime.UtcNow, Note = "Some note" };
            var fieldWorkParkingImgs = new List<Domain.Entities.FieldWorkParkingImg>
            {
                new Domain.Entities.FieldWorkParkingImg { Url = "img1.jpg", ApproveParkingId = 123 },
                new Domain.Entities.FieldWorkParkingImg { Url = "img2.jpg", ApproveParkingId = 123 }
            };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), true)).ReturnsAsync(approveParking);
            _mapperMock.Setup(x => x.Map<List<ImagesOfRequestApprove>>(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>())).Returns(new List<ImagesOfRequestApprove>());

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Data.ApproveParkingId.ShouldBe(123);
            result.Data.StaffId.ShouldBe(1);
            result.Data.Status.ShouldBe("Tạo_mới");
            result.Data.Note.ShouldBe("Some note");
            result.Data.CreatedDate.ShouldBe(approveParking.CreatedDate);
            _approveParkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), true), Times.Once);
        }
    }
}

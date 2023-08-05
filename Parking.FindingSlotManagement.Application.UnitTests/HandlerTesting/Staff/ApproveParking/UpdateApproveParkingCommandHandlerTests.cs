using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.UpdateApproveParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.ApproveParking
{
    public class UpdateApproveParkingCommandHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;

        private readonly UpdateApproveParkingCommandHandler _commandHandler;

        public UpdateApproveParkingCommandHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _commandHandler = new UpdateApproveParkingCommandHandler(_approveParkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateApproveParkingCommand { ApproveParkingId = 123, Note = "Some note" };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync((Domain.Entities.ApproveParking)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy yêu cầu xác thực của bãi.");
            _approveParkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestAlreadyProcessed_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateApproveParkingCommand { ApproveParkingId = 123, Note = "Some note" };
            var approveParking = new Domain.Entities.ApproveParking { ApproveParkingId = 123, Status = Domain.Enum.ApproveParkingStatus.Đã_duyệt.ToString() };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.");
            _approveParkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
        }
        [Fact]
        public async Task Handle_UpdateNote_Success()
        {
            // Arrange
            var request = new UpdateApproveParkingCommand { ApproveParkingId = 123, Note = "Some note" };
            var approveParking = new Domain.Entities.ApproveParking { ApproveParkingId = 123, Status = Domain.Enum.ApproveParkingStatus.Tạo_mới.ToString() };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            _approveParkingRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ApproveParking>()), Times.Once);
            approveParking.Note.ShouldBe("Some note");
        }

    }
}

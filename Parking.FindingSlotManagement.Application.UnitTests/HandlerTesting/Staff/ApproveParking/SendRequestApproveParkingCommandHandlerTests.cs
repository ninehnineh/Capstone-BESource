using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.SendRequestApproveParking;
using Parking.FindingSlotManagement.Domain.Entities;
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
    public class SendRequestApproveParkingCommandHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;

        private readonly SendRequestApproveParkingCommandHandler _commandHandler;

        public SendRequestApproveParkingCommandHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _commandHandler = new SendRequestApproveParkingCommandHandler(_approveParkingRepositoryMock.Object, _parkingHasPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new SendRequestApproveParkingCommand { ApproveParkingId = 123 };
            _approveParkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Domain.Entities.ApproveParking)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy yêu cầu.");
            _approveParkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_ApproveParkingRequestAlreadyApproved_ReturnsErrorResponse()
        {
            // Arrange
            var request = new SendRequestApproveParkingCommand { ApproveParkingId = 123 };
            var approveParking = new Domain.Entities.ApproveParking { ApproveParkingId = 123, Status = ApproveParkingStatus.Đã_duyệt.ToString() };
            _approveParkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(approveParking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Không thể gửi yêu cầu duyệt.");
            _approveParkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_UpdateStatusToWaitingApproval_Success()
        {
            // Arrange
            var request = new SendRequestApproveParkingCommand { ApproveParkingId = 123 };
            var approveParking = new Domain.Entities.ApproveParking { ApproveParkingId = 123, ParkingId = 1, Status = ApproveParkingStatus.Tạo_mới.ToString() };
            
            _approveParkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(approveParking);
            var parkingHasPriceExist = new ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = 1, ParkingPriceId = 1 };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<ParkingHasPrice, bool>>>(), null, true)).ReturnsAsync(parkingHasPriceExist);
            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            _approveParkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Save(), Times.Once);
            approveParking.Status.ShouldBe(ApproveParkingStatus.Chờ_duyệt.ToString());
        }
    }
}

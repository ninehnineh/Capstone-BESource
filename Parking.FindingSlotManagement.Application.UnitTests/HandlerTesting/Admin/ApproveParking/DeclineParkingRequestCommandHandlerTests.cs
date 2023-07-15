/*using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.DeclineParkingRequest;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ApproveParking
{
    public class DeclineParkingRequestCommandHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly DeclineParkingRequestCommandHandler _handler;
        public DeclineParkingRequestCommandHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new DeclineParkingRequestCommandHandler(_approveParkingRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenAllConditionsAreMet_ShouldReturnSuccessResponseAndSaveChanges()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };
            var approveParking = new Domain.Entities.ApproveParking { ParkingId = parkingId, Status = ApproveParkingStatus.Chờ_duyệt.ToString() };

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);
            _parkingRepositoryMock.Setup(r => r.Save()).Returns(Task.CompletedTask);

            _approveParkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false))
                .ReturnsAsync(approveParking);
            _approveParkingRepositoryMock.Setup(r => r.Save()).Returns(Task.CompletedTask);

            var command = new DeclineParkingRequestCommand { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);

            _parkingRepositoryMock.Verify(r => r.Save(), Times.Once);
            _approveParkingRepositoryMock.Verify(r => r.Save(), Times.Once);

            approveParking.Status.ShouldBe(ApproveParkingStatus.Từ_chối.ToString());
        }
        [Fact]
        public async Task Handle_WhenParkingDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var parkingId = 1;
            Domain.Entities.Parking parkingExist = null;

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            var command = new DeclineParkingRequestCommand { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_WhenParkingIsAlreadyApproved_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId, IsActive = true };

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            var command = new DeclineParkingRequestCommand { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Bãi đã được duyệt, không thể thực hiện thao tác.");
            response.StatusCode.ShouldBe(400);
        }
        [Fact]
        public async Task Handle_WhenApprovalRequestDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };
            Domain.Entities.ApproveParking approveParking = null;

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            _approveParkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false))
                .ReturnsAsync(approveParking);

            var command = new DeclineParkingRequestCommand { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy yêu cầu xác thực của bãi.");
            response.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_WhenApprovalRequestIsAlreadyProcessed_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };
            var approveParking = new Domain.Entities.ApproveParking { ParkingId = parkingId, Status = ApproveParkingStatus.Đã_duyệt.ToString() };

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            _approveParkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false))
                .ReturnsAsync(approveParking);

            var command = new DeclineParkingRequestCommand { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.");
            response.StatusCode.ShouldBe(400);
        }
    }
}
*/
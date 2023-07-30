using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.AcceptParkingRequest;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
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
    public class AcceptParkingRequestCommandHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AcceptParkingRequestCommandHandler _handler;
        public AcceptParkingRequestCommandHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _handler = new AcceptParkingRequestCommandHandler(_approveParkingRepositoryMock.Object, _userRepositoryMock.Object, _businessProfileRepositoryMock.Object, _parkingRepositoryMock.Object, _emailServiceMock.Object);
        }
        [Fact]
        public async Task Handle_WhenApproveParkingIsNull_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new AcceptParkingRequestCommand { ApproveParkingId = 1 };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync((Domain.Entities.ApproveParking)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(404);
            response.Message.ShouldBe("Không tìm thấy yêu cầu xác thực của bãi.");
        }
        [Fact]
        public async Task Handle_WhenApproveParkingIsAlreadyProcessed_ReturnsBadRequestResponse()
        {
            // Arrange
            var request = new AcceptParkingRequestCommand { ApproveParkingId = 1 };
            var approveParking = new Domain.Entities.ApproveParking
            {
                ApproveParkingId = 1,
                Status = Domain.Enum.ApproveParkingStatus.Đã_duyệt.ToString()
            };
            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.");
        }
        [Fact]
        public async Task Handle_WhenParkingApprovalIsSuccessful_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new AcceptParkingRequestCommand { ApproveParkingId = 1 };
            var approveParking = new Domain.Entities.ApproveParking
            {
                ApproveParkingId = 1,
                Status = Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString(),
                ParkingId = 1
            };
            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, IsActive = false, BusinessId = 1 };
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = 1, UserId = 1 };
            var userExist = new Domain.Entities.User { UserId = 1, Email = "test@example.com", Name = "John Doe" };

            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);
            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(parkingExist);
            _businessProfileRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(businessExist);
            _userRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(userExist);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(204);
            response.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WhenApproveParkingHasNotFoundBusiness_ReturnsBadRequestResponse()
        {
            var request = new AcceptParkingRequestCommand { ApproveParkingId = 1 };
            var approveParking = new Domain.Entities.ApproveParking
            {
                ApproveParkingId = 1,
                Status = Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString(),
                ParkingId = 1
            };
            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, IsActive = false, BusinessId = 1 };
            

            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);
            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(parkingExist);
            _businessProfileRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Domain.Entities.BusinessProfile)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(404);
            response.Message.ShouldBe("Không tìm thấy doanh nghiệp.");
        }
        [Fact]
        public async Task Handle_WhenApproveParkingHasNotFoundUser_ReturnsBadRequestResponse()
        {
            var request = new AcceptParkingRequestCommand { ApproveParkingId = 1 };
            var approveParking = new Domain.Entities.ApproveParking
            {
                ApproveParkingId = 1,
                Status = Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString(),
                ParkingId = 1
            };
            var parkingExist = new Domain.Entities.Parking { ParkingId = 1, IsActive = false, BusinessId = 1 };
            var businessExist = new Domain.Entities.BusinessProfile { BusinessProfileId = 1, UserId = 1 };

            _approveParkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), null, false)).ReturnsAsync(approveParking);
            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(parkingExist);
            _businessProfileRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(businessExist);
            _userRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((User)null);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(404);
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
    }
}

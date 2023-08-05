using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking;
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
    public class CreateNewApproveParkingCommandHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly CreateNewApproveParkingCommandHandler _commandHandler;

        public CreateNewApproveParkingCommandHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _mapperMock = new Mock<IMapper>();
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _commandHandler = new CreateNewApproveParkingCommandHandler(
                _approveParkingRepositoryMock.Object,
                _userRepositoryMock.Object,
                _parkingRepositoryMock.Object,
                _mapperMock.Object,
                _fieldWorkParkingImgRepositoryMock.Object
            );
        }
        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var staffId = 123;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId };
            var expectedResponse = new ServiceResponse<int>
            {
                Success = true,
                StatusCode = 200,
                Message = "Không tìm thấy tài khoản."
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_StaffRoleNotValid_ReturnsErrorResponse()
        {
            // Arrange
            var staffId = 123;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId };
            var staffUser = new User { UserId = staffId, RoleId = 3 }; // RoleId not equal to 4 (not staff)

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tài khoản của bạn không phải là thực địa của hệ thông. Không được phép truy cập.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ParkingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var staffId = 123;
            var parkingId = 456;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId, ParkingId = parkingId };
            var staffUser = new User { UserId = staffId, RoleId = 4 };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);
            _parkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _parkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ParkingAlreadyActive_ReturnsErrorResponse()
        {
            // Arrange
            var staffId = 123;
            var parkingId = 456;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId, ParkingId = parkingId };
            var staffUser = new User { UserId = staffId, RoleId = 4 };
            var parking = new Domain.Entities.Parking { ParkingId = parkingId, IsActive = true };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);
            _parkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(parking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Bãi đã được xác thực.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _parkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>()), Times.Never);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_CreateNewApproveParking_Success()
        {
            // Arrange
            var staffId = 123;
            var parkingId = 456;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId, ParkingId = parkingId, Images = new List<string> { "image1.jpg", "image2.jpg" } };
            var staffUser = new User { UserId = staffId, RoleId = 4 };
            var parking = new Domain.Entities.Parking { ParkingId = parkingId, IsActive = false };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);
            _parkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(parking);

            var insertedApproveParkingId = 789;
            /*var expectedResponse = new ServiceResponse<int>
            {
                Data = insertedApproveParkingId,
                Success = true,
                StatusCode = 201,
                Message = "Thành công"
            };*/

            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>())).ReturnsAsync((List<Domain.Entities.ApproveParking>)null);
            _mapperMock.Setup(x => x.Map<Domain.Entities.ApproveParking>(It.IsAny<CreateNewApproveParkingCommand>())).Returns(new Domain.Entities.ApproveParking());
            _approveParkingRepositoryMock.Setup(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>())).Returns(Task.CompletedTask);
            _fieldWorkParkingImgRepositoryMock.Setup(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBe(true);
            result.StatusCode.ShouldBe(201);
            result.Message.ShouldBe("Thành công");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _parkingRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Domain.Entities.ApproveParking>(It.IsAny<CreateNewApproveParkingCommand>()), Times.Once);
            _approveParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>()), Times.Once);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Once);
        }
        [Fact]
        public async Task Handle_CreateNewApproveParking_Error_When_do_not_have_images()
        {
            // Arrange
            var staffId = 123;
            var parkingId = 456;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId, ParkingId = parkingId, Images = new List<string> { } };
            var staffUser = new User { UserId = staffId, RoleId = 4 };
            var parking = new Domain.Entities.Parking { ParkingId = parkingId, IsActive = false };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);
            _parkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(parking);

            var insertedApproveParkingId = 789;
            /*var expectedResponse = new ServiceResponse<int>
            {
                Data = insertedApproveParkingId,
                Success = true,
                StatusCode = 201,
                Message = "Thành công"
            };*/

            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>())).ReturnsAsync((List<Domain.Entities.ApproveParking>)null);
            _mapperMock.Setup(x => x.Map<Domain.Entities.ApproveParking>(It.IsAny<CreateNewApproveParkingCommand>())).Returns(new Domain.Entities.ApproveParking());
            _approveParkingRepositoryMock.Setup(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>())).Returns(Task.CompletedTask);
            _fieldWorkParkingImgRepositoryMock.Setup(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBe(false);
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Hãy nhập ảnh thực địa về bãi xe.");
        }
        [Fact]
        public async Task Handle_CreateNewApproveParking_Error_When_already_have_request()
        {
            // Arrange
            var staffId = 123;
            var parkingId = 456;
            var request = new CreateNewApproveParkingCommand { StaffId = staffId, ParkingId = parkingId, Images = new List<string> { "image1.jpg", "image2.jpg" } };
            var staffUser = new User { UserId = staffId, RoleId = 4 };
            var parking = new Domain.Entities.Parking { ParkingId = parkingId, IsActive = false };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(staffUser);
            _parkingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(parking);

            var insertedApproveParkingId = 789;
            /*var expectedResponse = new ServiceResponse<int>
            {
                Data = insertedApproveParkingId,
                Success = true,
                StatusCode = 201,
                Message = "Thành công"
            };*/
            List<Domain.Entities.ApproveParking> test = new List<Domain.Entities.ApproveParking> { new Domain.Entities.ApproveParking { ApproveParkingId = 1, Status = ApproveParkingStatus.Tạo_mới.ToString() } };
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithConditionByNoInclude(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>())).ReturnsAsync(test);
            _mapperMock.Setup(x => x.Map<Domain.Entities.ApproveParking>(It.IsAny<CreateNewApproveParkingCommand>())).Returns(new Domain.Entities.ApproveParking());
            _approveParkingRepositoryMock.Setup(x => x.Insert(It.IsAny<Domain.Entities.ApproveParking>())).Returns(Task.CompletedTask);
            _fieldWorkParkingImgRepositoryMock.Setup(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBe(false);
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Đang có yêu cầu tạo mới, không thể tạo thêm yêu cầu.");
        }
    }
}

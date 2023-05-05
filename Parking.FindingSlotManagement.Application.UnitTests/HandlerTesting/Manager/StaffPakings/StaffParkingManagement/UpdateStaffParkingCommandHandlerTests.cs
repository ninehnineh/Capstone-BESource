using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.UpdateStaffParking;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.StaffPakings.StaffParkingManagement
{
    public class UpdateStaffParkingCommandHandlerTests
    {
        private readonly Mock<IStaffParkingRepository> _staffParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly UpdateStaffParkingCommandHandler _handler;
        public UpdateStaffParkingCommandHandlerTests()
        {
            _staffParkingRepositoryMock = new Mock<IStaffParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new UpdateStaffParkingCommandHandler(_staffParkingRepositoryMock.Object, _accountRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateStaffParkingCommandHandler_Should_Update_StaffParking_Successfully()
        {
            // Arrange
            var request = new UpdateStaffParkingCommand
            {
                StaffParkingId = 1,
                UserId = 7,
                ParkingId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldStaffParking = new Domain.Entities.StaffParking
            {
                StaffParkingId = 1,
                UserId = 10,
                ParkingId = 5
            };
            _staffParkingRepositoryMock.Setup(x => x.GetById(request.StaffParkingId))
                .ReturnsAsync(OldStaffParking);
            var userExist = new Domain.Entities.User { UserId = 10 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(userExist);
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldStaffParking.UserId.ShouldBe(request.UserId);
            OldStaffParking.ParkingId.ShouldBe(request.ParkingId);
            _staffParkingRepositoryMock.Verify(x => x.Update(OldStaffParking), Times.Once);
        }
        [Fact]
        public async Task UpdateStaffParkingCommandHandler_Should_Return_Not_Found_If_StaffParking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateStaffParkingCommand
            {
                StaffParkingId = 2000
            };
            var cancellationToken = new CancellationToken();
            _staffParkingRepositoryMock.Setup(x => x.GetById(request.StaffParkingId))
                .ReturnsAsync((StaffParking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _staffParkingRepositoryMock.Verify(x => x.Update(It.IsAny<StaffParking>()), Times.Never);
        }
        [Fact]
        public async Task UpdateStaffParkingCommandHandler_Should_Return_Not_Found_If_User_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateStaffParkingCommand
            {
                StaffParkingId = 1,
                UserId = 2,
                ParkingId = 5
            };
            var cancellationToken = new CancellationToken();
            var staffParking = new StaffParking { StaffParkingId = 1 };
            _staffParkingRepositoryMock.Setup(x => x.GetById(request.StaffParkingId))
                .ReturnsAsync(staffParking);
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(parkingExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _staffParkingRepositoryMock.Verify(x => x.Update(It.IsAny<StaffParking>()), Times.Never);
        }
        [Fact]
        public async Task UpdateStaffParkingCommandHandler_Should_Return_Not_Found_If_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateStaffParkingCommand
            {
                StaffParkingId = 1,
                UserId = 7,
                ParkingId = 1
            };
            var cancellationToken = new CancellationToken();
            var staffParking = new StaffParking { StaffParkingId = 1 };
            _staffParkingRepositoryMock.Setup(x => x.GetById(request.StaffParkingId))
                .ReturnsAsync(staffParking);
            var userExist = new User { UserId = 5 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(userExist);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _staffParkingRepositoryMock.Verify(x => x.Update(It.IsAny<StaffParking>()), Times.Never);
        }
    }
}

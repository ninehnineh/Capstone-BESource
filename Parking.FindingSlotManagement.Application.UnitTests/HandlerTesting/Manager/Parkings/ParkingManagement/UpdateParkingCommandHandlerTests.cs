using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class UpdateParkingCommandHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly UpdateParkingCommandHandler _handler;
        private readonly UpdateParkingCommandValidation _validator;
        public UpdateParkingCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new UpdateParkingCommandValidation();
            _handler = new UpdateParkingCommandHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateParkingCommandHandler_Should_Update_Parking_Successfully()
        {
            // Arrange
            var request = new UpdateParkingCommand
            {
                ParkingId = 5,
                Name = "Bãi xe Hung Lam",
                Address = "12 Công xã Paris, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };
            var cancellationToken = new CancellationToken();
            var OldParking = new Domain.Entities.Parking
            {
                ParkingId = 5,
                Name = "Bãi xe nhà thờ Đức Bàm",
                Address = "01 Công xã Paris, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(OldParking);
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true))
                .ReturnsAsync((Domain.Entities.Parking)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldParking.Name.ShouldBe(request.Name);
            OldParking.Address.ShouldBe(request.Address);
            OldParking.Description.ShouldBe(request.Description);
            OldParking.MotoSpot.ShouldBe(request.MotoSpot);
            OldParking.CarSpot.ShouldBe(request.CarSpot);
            OldParking.IsPrepayment.ShouldBe(request.IsPrepayment);
            OldParking.IsOvernight.ShouldBe(request.IsOvernight);
            _parkingRepositoryMock.Verify(x => x.Update(OldParking), Times.Once);
        }
        [Fact]
        public async Task UpdateParkingCommandHandler_Should_Return_Not_Found_If_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingCommand
            {
                ParkingId = 2000
            };
            var cancellationToken = new CancellationToken();
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Parking>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingCommandHandler_Should_Return_Not_Found_If_ParkingName_Has_Existed()
        {
            // Arrange
            var request = new UpdateParkingCommand
            {
                ParkingId = 5,
                Name = "Bãi xe Hung Lam",
                Address = "12 Công xã Paris, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };
            var cancellationToken = new CancellationToken();
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(parkingExist);
            var parkingExistName = new Domain.Entities.Parking { Name = "Bãi xe Hung Lam" };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true))
                .ReturnsAsync(parkingExistName);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Tên bãi đã tồn tại. Vui lòng nhập tên bãi khác.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Parking>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateParkingCommand
            {
                ParkingId = 5,
                Name = "Bãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung Lam",
                Address = "12 Công xã Paris, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateParkingCommand
            {
                ParkingId = 5,
                Name = "Bãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung Lam",
                Address = "12 Công xã Paris, Bến NghBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung Lamé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Description_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateParkingCommand
            {
                ParkingId = 5,
                Name = "Bãi xe Hung Lam",
                Address = "12 Công xã Paris, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh 70000",
                Description = "Đây là một bãi xe rBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamvBãi xe Hung LamBãi xe Hung LamBãi xe Hung LamBãi xe Hung Lamộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}

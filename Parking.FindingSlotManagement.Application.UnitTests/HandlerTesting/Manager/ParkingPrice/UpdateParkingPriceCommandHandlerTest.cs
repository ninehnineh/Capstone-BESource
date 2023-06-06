/*using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.UpdateParkingPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class UpdateParkingPriceCommandHandlerTest
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly UpdateParkingPriceCommandHandler _handler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly UpdateParkingPriceCommandValidation _validator;

        public UpdateParkingPriceCommandHandlerTest()
        {
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _validator = new UpdateParkingPriceCommandValidation(_parkingPriceRepositoryMock.Object);
            _handler = new UpdateParkingPriceCommandHandler(_parkingPriceRepositoryMock.Object, _userRepositoryMock.Object, _trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Update_ParkingPrice_Successfully()
        {
            // Arrange
            var request = new UpdateParkingPriceCommand
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói cho xe ô tô",
                BusinessId = 1,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldParkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói 1",
                IsActive = true,
                UserId = 1,
                TrafficId = 1,
                IsWholeDay = false
            };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false))
                .ReturnsAsync(OldParkingPrice);
            var BusinessExist = new Domain.Entities.User { UserId = 1, RoleId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(request.BusinessId)).ReturnsAsync(BusinessExist);
            var trafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldParkingPrice.ParkingPriceName.ShouldBe(request.ParkingPriceName);
            _parkingPriceRepositoryMock.Verify(x => x.Update(OldParkingPrice), Times.Once);
        }
        [Fact]
        public async Task Should_Have_Error_When_ParkingPrice_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingPriceCommand
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói cho xe ô tô",
                BusinessId = 1,
                TrafficId = 1
            }; 

            // Act
            var result = await _validator.TestValidateAsync(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceId)
                .WithErrorMessage("'Parking Price Id' không tồn tại")
                .WithSeverity(Severity.Error);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Return_Not_Found_If_Business_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingPriceCommand
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói cho xe ô tô",
                BusinessId = 1,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldParkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói 1",
                IsActive = true,
                UserId = 1,
                TrafficId = 1,
                IsWholeDay = false
            };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false))
                .ReturnsAsync(OldParkingPrice);
            _userRepositoryMock.Setup(x => x.GetById(request.BusinessId)).ReturnsAsync((User)null);
            var trafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            // Act
            var response = await _handler.Handle(request, cancellationToken);
            

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản doanh nghiệp.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Return_Not_Found_If_Role_Is_Not_A_Manager()
        {
            // Arrange
            var request = new UpdateParkingPriceCommand
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói cho xe ô tô",
                BusinessId = 1,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldParkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói 1",
                IsActive = true,
                UserId = 1,
                TrafficId = 1,
                IsWholeDay = false
            };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false))
                .ReturnsAsync(OldParkingPrice);
            var BusinessExist = new Domain.Entities.User { UserId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(request.BusinessId)).ReturnsAsync(BusinessExist);
            var trafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);
            // Act
            var response = await _handler.Handle(request, cancellationToken);
            

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Tài khoản không phải doanh nghiệp.");
            response.StatusCode.ShouldBe(400);
            response.Count.ShouldBe(0);
            _parkingPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingPrice>()), Times.Never);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Return_Not_Found_If_Traffic_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingPriceCommand
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói cho xe ô tô",
                BusinessId = 1,
                TrafficId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldParkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói 1",
                IsActive = true,
                UserId = 1,
                TrafficId = 1,
                IsWholeDay = false
            };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false))
                .ReturnsAsync(OldParkingPrice);
            var BusinessExist = new Domain.Entities.User { UserId = 1, RoleId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(request.BusinessId)).ReturnsAsync(BusinessExist);
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync((Traffic)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);


            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy phương tiện.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingPriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingPrice>()), Times.Never);
        }
        [Fact]
        public async Task Should_Have_Error_When_ParkingPriceName_Is_LessThan_250()
        {
            // Arrange
            var command = new UpdateParkingPriceCommand { BusinessId = 1, ParkingPriceName = "testtesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestttesttesttesttesttesttesttesttesttesttestesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttestt", TrafficId = 1 };
            var checkUserExist = new User { UserId = 1 };
            var cancellationToken = new CancellationToken();
            var OldParkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = "Gói 1",
                IsActive = true,
                UserId = 1,
                TrafficId = 1,
                IsWholeDay = false
            };
            _parkingPriceRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ParkingPriceId == command.ParkingPriceId, null, false))
                .ReturnsAsync(OldParkingPrice);
            var BusinessExist = new Domain.Entities.User { UserId = 1, RoleId = 1 };
            _userRepositoryMock.Setup(x => x.GetById(command.BusinessId)).ReturnsAsync(BusinessExist);
            var trafficExist = new Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(trafficExist);
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceName)
                .WithErrorMessage("Parking Price Name không được nhập quá 250 kí tự")
                .WithSeverity(Severity.Error);
        }
    }
}
*/
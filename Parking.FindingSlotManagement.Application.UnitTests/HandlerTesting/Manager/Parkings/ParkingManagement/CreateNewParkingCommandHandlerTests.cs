using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class CreateNewParkingCommandHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly CreateNewParkingCommandHandler _handler;
        private readonly CreateNewParkingCommandValidation _validator;

        public CreateNewParkingCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new CreateNewParkingCommandValidation();
            _handler = new CreateNewParkingCommandHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewParkingCommand 
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };
            var expectedParking = new Domain.Entities.Parking 
            {
                ParkingId = 6,
                Code = "BX6",
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync((Domain.Entities.Parking)null);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            // Verify that the account repository was called to insert the new account
            _parkingRepositoryMock.Verify(x => x.Insert(It.Is<Domain.Entities.Parking>(parking => parking.Name == parking.Name)));
        }
        [Fact]
        public async Task Handle_ParkingNameExists_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };
            var existedParking = new Domain.Entities.Parking
            {
                ParkingId = 6,
                Code = "BX6",
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };
            _parkingRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true)).ReturnsAsync(existedParking);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Tên bãi xe đã tồn tại. Vui lòng nhập tên bãi xe khác.");
            result.StatusCode.ShouldBe(400);

            _parkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.Parking>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc Bãi Giữ Xe Minh Phúc Bãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Address_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Description_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho nhữngĐây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void MotoSpot_ShouldNotLessThanZero()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = -50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.MotoSpot);
        }
        [Fact]
        public void CarSpot_ShouldNotLessThanZero()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = -50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.CarSpot);
        }
        [Fact]
        public void IsPrepayment_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = null,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.IsPrepayment);
        }
        [Fact]
        public void IsPrepayment_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsOvernight = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.IsPrepayment);
        }
        [Fact]
        public void IsOvernight_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = null,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.IsOvernight);
        }
        [Fact]
        public void IsOvernight_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                ManagerId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.IsOvernight);
        }
        [Fact]
        public void ManagerId_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
                ManagerId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ManagerId);
        }
        [Fact]
        public void ManagerId_ShouldNotBeNull()
        {
            var command = new CreateNewParkingCommand()
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                Description = "Đây là một bãi xe rộng lớn, có thể đỗ được nhiều loại xe khác nhau, bao gồm cả ô tô và xe máy. Vị trí của bãi xe rất thuận tiện cho những người muốn đến tham quan Nhà Thờ Đức Bà, một trong những công trình kiến trúc nổi tiếng của thành phố.",
                MotoSpot = 50,
                CarSpot = 50,
                IsPrepayment = true,
                IsOvernight = true,
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ManagerId);
        }
    }
}

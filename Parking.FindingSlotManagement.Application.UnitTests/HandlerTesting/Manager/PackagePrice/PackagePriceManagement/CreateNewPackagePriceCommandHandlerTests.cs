/*using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.CreateNewPackagePrice;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.PackagePrice.PackagePriceManagement
{
    public class CreateNewPackagePriceCommandHandlerTests
    {
        private readonly Mock<IPackagePriceRepository> _packagePriceRepositoryMock;
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly CreateNewPackagePriceCommandValidation _validator;
        private readonly CreateNewPackagePriceCommandHandler _handler;
        public CreateNewPackagePriceCommandHandlerTests()
        {
            _packagePriceRepositoryMock = new Mock<IPackagePriceRepository>();
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _validator = new CreateNewPackagePriceCommandValidation();
            _handler = new CreateNewPackagePriceCommandHandler(_packagePriceRepositoryMock.Object, _trafficRepositoryMock.Object);
        }
        *//*[Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(trafficExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Once);
        }*//*

        [Fact]
        public async Task Handle_InvalidTrafficId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync((Domain.Entities.Traffic)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy phương tiện.");
            result.StatusCode.ShouldBe(200);

            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidStartTime_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddDays(-1).AddHours(6),
                EndTime = DateTime.Now.Date.AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(trafficExist);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.");
            result.StatusCode.ShouldBe(400);

            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidEndTime_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var trafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(trafficExist);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.");
            result.StatusCode.ShouldBe(400);

            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Price_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = null,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Price_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Price_Should_Greater_Than_Zero()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = -20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Description_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void Description_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
        [Fact]
        public void StartTime_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = null,
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.StartTime);
        }
        [Fact]
        public void StartTime_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.StartTime);
        }
        [Fact]
        public void EndTime_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = null,
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.EndTime);
        }
        [Fact]
        public void EndTime_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.EndTime);
        }
        [Fact]
        public void TrafficId_ShouldNotBeEmpty()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
                TrafficId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TrafficId);
        }
        [Fact]
        public void TrafficId_ShouldNotBeNull()
        {
            var command = new CreateNewPackagePriceCommand
            {
                Name = "Gói ngày cho xe ô tô",
                Price = 20000,
                Description = "Gói giữ xe ban ngày cho xe ô tô",
                StartTime = DateTime.Now.Date.AddHours(6),
                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
                IsExtrafee = true,
                ExtraFee = 5000,
                ExtraTimeStep = 1,
                HasPenaltyPrice = true,
                PenaltyPrice = 20000,
                PenaltyPriceStepTime = 1,
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TrafficId);
        }
    }
}
*/
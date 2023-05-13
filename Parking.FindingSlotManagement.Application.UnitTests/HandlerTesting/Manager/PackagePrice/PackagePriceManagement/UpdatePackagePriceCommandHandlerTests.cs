//using FluentValidation.TestHelper;
//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.UpdatePackagePrice;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.PackagePrice.PackagePriceManagement
//{
//    public class UpdatePackagePriceCommandHandlerTests
//    {
//        private readonly Mock<IPackagePriceRepository> _packagePriceRepositoryMock;
//        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
//        private readonly UpdatePackagePriceCommandValidation _validator;
//        private readonly UpdatePackagePriceCommandHandler _handler;
//        public UpdatePackagePriceCommandHandlerTests()
//        {
//            _packagePriceRepositoryMock = new Mock<IPackagePriceRepository>();
//            _trafficRepositoryMock = new Mock<ITrafficRepository>();
//            _validator = new UpdatePackagePriceCommandValidation();
//            _handler = new UpdatePackagePriceCommandHandler(_packagePriceRepositoryMock.Object, _trafficRepositoryMock.Object);
//        }
//        [Fact]
//        public async Task UpdatePackagePriceCommandHandler_Should_Update_PackagePrice_Successfully()
//        {
//            // Arrange
//            var request = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            var cancellationToken = new CancellationToken();
//            var OldPackagePrice = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(6),
//                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ReturnsAsync(OldPackagePrice);
//            var TrafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
//            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync(TrafficExist);

//            // Act
//            var response = await _handler.Handle(request, cancellationToken);

//            // Assert
//            response.ShouldNotBeNull();
//            response.Success.ShouldBeTrue();
//            response.Message.ShouldBe("Thành công");
//            response.StatusCode.ShouldBe(204);
//            response.Count.ShouldBe(0);
//            OldPackagePrice.Name.ShouldBe(request.Name);
//            OldPackagePrice.Price.ShouldBe(request.Price);
//            OldPackagePrice.Description.ShouldBe(request.Description);
//            OldPackagePrice.StartTime.ShouldBe(request.StartTime);
//            OldPackagePrice.EndTime.ShouldBe(request.EndTime);
//            OldPackagePrice.IsExtrafee.ShouldBe(request.IsExtrafee);
//            OldPackagePrice.ExtraFee.ShouldBe(request.ExtraFee);
//            OldPackagePrice.ExtraTimeStep.ShouldBe(request.ExtraTimeStep);
//            OldPackagePrice.HasPenaltyPrice.ShouldBe(request.HasPenaltyPrice);
//            OldPackagePrice.PenaltyPrice.ShouldBe(request.PenaltyPrice);
//            OldPackagePrice.PenaltyPriceStepTime.ShouldBe(request.PenaltyPriceStepTime);
//            OldPackagePrice.TrafficId.ShouldBe(request.TrafficId);
//            _packagePriceRepositoryMock.Verify(x => x.Update(OldPackagePrice), Times.Once);
//        }
//        [Fact]
//        public async Task UpdatePackagePriceCommandHandler_Should_Return_Not_Found_If_PackagePrice_Does_Not_Exist()
//        {
//            // Arrange
//            var request = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 999999,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            var cancellationToken = new CancellationToken();
//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ReturnsAsync((Domain.Entities.TimeLine)null);

//            // Act
//            var response = await _handler.Handle(request, cancellationToken);

//            // Assert
//            response.ShouldNotBeNull();
//            response.Success.ShouldBeTrue();
//            response.Message.ShouldBe("Không tìm thấy gói.");
//            response.StatusCode.ShouldBe(200);
//            response.Count.ShouldBe(0);
//            _packagePriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
//        }
//        [Fact]
//        public async Task UpdatePackagePriceCommandHandler_Should_Return_Not_Found_If_Traffic_Does_Not_Exist()
//        {
//            // Arrange
//            var request = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            var cancellationToken = new CancellationToken();
//            var OldPackagePrice = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(6),
//                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ReturnsAsync(OldPackagePrice);
//            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId)).ReturnsAsync((Domain.Entities.Traffic)null);

//            // Act
//            var response = await _handler.Handle(request, cancellationToken);

//            // Assert
//            response.ShouldNotBeNull();
//            response.Success.ShouldBeTrue();
//            response.Message.ShouldBe("Không tìm thấy phương tiện.");
//            response.StatusCode.ShouldBe(200);
//            response.Count.ShouldBe(0);
//            _packagePriceRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
//        }
//        [Fact]
//        public async Task Handle_InvalidStartTime_ReturnsErrorResponse()
//        {
//            // Arrange
//            var command = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddDays(-1).AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            var cancellationToken = new CancellationToken();
//            var OldPackagePrice = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(6),
//                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            _packagePriceRepositoryMock.Setup(x => x.GetById(command.PackagePriceId))
//                .ReturnsAsync(OldPackagePrice);
//            var trafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
//            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(trafficExist);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeFalse();
//            result.Message.ShouldBe("Ngày giờ bắt đầu phải bắt đầu trong khoảng từ 0h ngày hôm nay.");
//            result.StatusCode.ShouldBe(400);

//            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
//        }
//        [Fact]
//        public async Task Handle_InvalidEndTime_ReturnsErrorResponse()
//        {
//            // Arrange
//            var command = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddDays(3).AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            var cancellationToken = new CancellationToken();
//            var OldPackagePrice = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(6),
//                EndTime = DateTime.Now.Date.AddDays(3).AddHours(18),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };
//            _packagePriceRepositoryMock.Setup(x => x.GetById(command.PackagePriceId))
//                .ReturnsAsync(OldPackagePrice);
//            var trafficExist = new Domain.Entities.Traffic { TrafficId = 1 };
//            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId)).ReturnsAsync(trafficExist);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeFalse();
//            result.Message.ShouldBe("Ngày giờ kết thúc không được vượt quá 1 ngày. Chỉ được set giờ cho đến ngày hôm sau.");
//            result.StatusCode.ShouldBe(400);

//            _packagePriceRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.TimeLine>()), Times.Never);
//        }
//        [Fact]
//        public void Name_ShouldNotExceedMaximumLength()
//        {
//            var command = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tôGói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Name);
//        }
//        [Fact]
//        public void Price_ShouldNotExceedMaximumLength()
//        {
//            var command = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = -20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Price);
//        }
//        [Fact]
//        public void Description_ShouldNotExceedMaximumLength()
//        {
//            var command = new UpdatePackagePriceCommand
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tôGói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Description);
//        }
//    }
//}

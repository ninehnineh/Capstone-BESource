//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.DisableOrEnablePackagePrice;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.PackagePrice.PackagePriceManagement
//{
//    public class DisableOrEnablePackagePriceCommandHandlerTests
//    {
//        private readonly Mock<IPackagePriceRepository> _packagePriceRepositoryMock;
//        private readonly DisableOrEnablePackagePriceCommandHandler _handler;
//        public DisableOrEnablePackagePriceCommandHandlerTests()
//        {
//            _packagePriceRepositoryMock = new Mock<IPackagePriceRepository>();
//            _handler = new DisableOrEnablePackagePriceCommandHandler(_packagePriceRepositoryMock.Object);
//        }
//        [Fact]
//        public async Task Handle_Should_Return_Successful_Response()
//        {
//            // Arrange
//            var request = new DisableOrEnablePackagePriceCommand
//            {
//                PackagePriceId = 1
//            };

//            var packagePriceToDelete = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = request.PackagePriceId,
//                IsActive = true,
//            };

//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ReturnsAsync(packagePriceToDelete);

//            _packagePriceRepositoryMock.Setup(x => x.Save())
//                .Returns(Task.CompletedTask);

//            // Act
//            var result = await _handler.Handle(request, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.Message.ShouldBe("Thành công");
//            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
//            result.Count.ShouldBe(0);

//            _packagePriceRepositoryMock.Verify(x => x.GetById(request.PackagePriceId), Times.Once);
//            _packagePriceRepositoryMock.Verify(x => x.Save(), Times.Once);
//        }
//        [Fact]
//        public async Task Handle_Should_Return_NotFound_Response_When_Package_Not_Found()
//        {
//            // Arrange
//            var request = new DisableOrEnablePackagePriceCommand
//            {
//                PackagePriceId = 9999999
//            };

//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ReturnsAsync((Domain.Entities.TimeLine)null);

//            // Act
//            var result = await _handler.Handle(request, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.Message.ShouldBe("Không tìm thấy gói.");
//            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
//            result.Count.ShouldBe(0);

//            _packagePriceRepositoryMock.Verify(x => x.GetById(request.PackagePriceId), Times.Once);
//            _packagePriceRepositoryMock.Verify(x => x.Save(), Times.Never);
//        }
//        [Fact]
//        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
//        {
//            // Arrange
//            var request = new DisableOrEnablePackagePriceCommand
//            {
//                PackagePriceId = 1
//            };

//            _packagePriceRepositoryMock.Setup(x => x.GetById(request.PackagePriceId))
//                .ThrowsAsync(new Exception("Some error message"));

//            // Act & Assert
//            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

//            _packagePriceRepositoryMock.Verify(x => x.GetById(request.PackagePriceId), Times.Once);
//            _packagePriceRepositoryMock.Verify(x => x.Save(), Times.Never);
//        }
//    }
//}

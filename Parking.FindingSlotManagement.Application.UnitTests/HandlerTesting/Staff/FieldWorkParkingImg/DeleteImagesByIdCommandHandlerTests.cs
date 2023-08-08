using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.DeleteImagesById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.FieldWorkParkingImg
{
    public class DeleteImagesByIdCommandHandlerTests
    {
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly DeleteImagesByIdCommandHandler _commandHandler;

        public DeleteImagesByIdCommandHandlerTests()
        {
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _commandHandler = new DeleteImagesByIdCommandHandler(_fieldWorkParkingImgRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidImageId_DeletesImageAndReturnsSuccessResponse()
        {
            // Arrange
            int imageIdToDelete = 101;
            var request = new DeleteImagesByIdCommand
            {
                FieldWorkParkingImgId = imageIdToDelete
            };
            var existingImage = new Domain.Entities.FieldWorkParkingImg { FieldWorkParkingImgId = imageIdToDelete };
            _fieldWorkParkingImgRepositoryMock.Setup(x => x.GetById(imageIdToDelete)).ReturnsAsync(existingImage);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.Delete(existingImage), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidImageId_ReturnsErrorResponse()
        {
            // Arrange
            int imageIdToDelete = 101;
            var request = new DeleteImagesByIdCommand
            {
                FieldWorkParkingImgId = imageIdToDelete
            };
            _fieldWorkParkingImgRepositoryMock.Setup(x => x.GetById(imageIdToDelete)).ReturnsAsync((Domain.Entities.FieldWorkParkingImg)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.FieldWorkParkingImg>()), Times.Never);
        }
    }
}

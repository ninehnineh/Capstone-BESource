using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.CreateNewImage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.FieldWorkParkingImg
{
    public class CreateNewImageCommandHandlerTests
    {
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly CreateNewImageCommandHandler _commandHandler;

        public CreateNewImageCommandHandlerTests()
        {
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _commandHandler = new CreateNewImageCommandHandler(
                _fieldWorkParkingImgRepositoryMock.Object,
                _approveParkingRepositoryMock.Object
            );
        }
        [Fact]
        public async Task Handle_ValidRequest_AddsImagesAndReturnsSuccessResponse()
        {
            // Arrange
            int approveParkingId = 101;
            var request = new CreateNewImageCommand
            {
                ApproveParkingId = approveParkingId,
                Images = new List<string> { "image_url_1", "image_url_2" }
            };
            var existingApproveParking = new Domain.Entities.ApproveParking { ApproveParkingId = approveParkingId };
            _approveParkingRepositoryMock.Setup(x => x.GetById(approveParkingId)).ReturnsAsync(existingApproveParking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Message.ShouldBe("Thành công");
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Once);
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.Is<List<Domain.Entities.FieldWorkParkingImg>>(
                lst => lst.Count == 2
                    && lst[0].ApproveParkingId == approveParkingId && lst[0].Url == "image_url_1"
                    && lst[1].ApproveParkingId == approveParkingId && lst[1].Url == "image_url_2"
            )), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidApproveParking_ReturnsErrorResponse()
        {
            // Arrange
            int approveParkingId = 101;
            var request = new CreateNewImageCommand
            {
                ApproveParkingId = approveParkingId,
                Images = new List<string> { "image_url_1", "image_url_2" }
            };
            _approveParkingRepositoryMock.Setup(x => x.GetById(approveParkingId)).ReturnsAsync((Domain.Entities.ApproveParking)null);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy yêu cầu.");
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_NoImages_ReturnsSuccessResponse()
        {
            // Arrange
            int approveParkingId = 101;
            var request = new CreateNewImageCommand
            {
                ApproveParkingId = approveParkingId,
                Images = new List<string>()
            };
            var existingApproveParking = new Domain.Entities.ApproveParking { ApproveParkingId = approveParkingId };
            _approveParkingRepositoryMock.Setup(x => x.GetById(approveParkingId)).ReturnsAsync(existingApproveParking);

            // Act
            var result = await _commandHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Message.ShouldBe("Thành công");
            _fieldWorkParkingImgRepositoryMock.Verify(x => x.AddRangeFieldWorkParkingImg(It.IsAny<List<Domain.Entities.FieldWorkParkingImg>>()), Times.Never);
        }

    }
}

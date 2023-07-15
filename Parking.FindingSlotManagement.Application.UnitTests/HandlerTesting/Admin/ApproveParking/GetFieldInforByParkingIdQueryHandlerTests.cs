/*using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ApproveParking
{
    public class GetFieldInforByParkingIdQueryHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly GetFieldInforByParkingIdQueryHandler _handler;
        public GetFieldInforByParkingIdQueryHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _handler = new GetFieldInforByParkingIdQueryHandler(_parkingRepositoryMock.Object, _approveParkingRepositoryMock.Object, _fieldWorkParkingImgRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenParkingDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var parkingId = 1;

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync((Domain.Entities.Parking)null);


            var query = new GetFieldInforByParkingIdQuery { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
            response.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_WhenNoImagesExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };
            var approveParkingInfo = new Domain.Entities.ApproveParking { ApproveParkingId = 1 };

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            _approveParkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), true))
                .ReturnsAsync(approveParkingInfo);

            _fieldWorkParkingImgRepositoryMock.Setup(r => r.GetAllItemWithConditionByNoInclude(It.IsAny <Expression<Func<Domain.Entities.FieldWorkParkingImg, bool>>> ()))
                .ReturnsAsync(new List<Domain.Entities.FieldWorkParkingImg>());


            var query = new GetFieldInforByParkingIdQuery { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Chưa có hình ảnh thực địa.");
            response.StatusCode.ShouldBe(400);
            response.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_WhenAllConditionsAreMet_ShouldReturnSuccessResponseWithFieldInformation()
        {
            // Arrange
            var parkingId = 1;
            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };
            var approveParkingInfo = new Domain.Entities.ApproveParking { ApproveParkingId = 1, Note = "Some note", StaffId = 1, User = new Domain.Entities.User { Name = "John Doe" } };
            var images = new List<Domain.Entities.FieldWorkParkingImg> { new Domain.Entities.FieldWorkParkingImg { Url = "image1.jpg" }, new Domain.Entities.FieldWorkParkingImg { Url = "image2.jpg" } };

            _parkingRepositoryMock.Setup(r => r.GetById(parkingId)).ReturnsAsync(parkingExist);

            _approveParkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(),true))
                .ReturnsAsync(approveParkingInfo);

            _fieldWorkParkingImgRepositoryMock.Setup(r => r.GetAllItemWithConditionByNoInclude(It.IsAny < Expression<Func<Domain.Entities.FieldWorkParkingImg, bool> >> ()))
                .ReturnsAsync(images);


            var query = new GetFieldInforByParkingIdQuery { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(200);
            response.Data.ShouldNotBeNull();
            response.Data.ApproveParkingId.ShouldBe(approveParkingInfo.ApproveParkingId);
            response.Data.Note.ShouldBe(approveParkingInfo.Note);
            response.Data.StaffId.ShouldBe(approveParkingInfo.StaffId);
            response.Data.StaffName.ShouldBe(approveParkingInfo.User.Name);
            response.Data.Images.ShouldBe(images.Select(img => img.Url));
        }
    }
}
*/
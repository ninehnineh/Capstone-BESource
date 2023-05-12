using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSpotImage.ParkingSpotImageManagement
{
    public class GetListImageByParkingIdQueryHandlerTests
    {
        private readonly Mock<IParkingSpotImageRepository> _parkingSpotImageRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetListImageByParkingIdQueryHandler _handler;
        public GetListImageByParkingIdQueryHandlerTests()
        {
            _parkingSpotImageRepositoryMock = new Mock<IParkingSpotImageRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetListImageByParkingIdQueryHandler(_parkingSpotImageRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListImageByParkingIdQuery
            {
                ParkingId = 5,
                PageNo = 1,
                PageSize = 10
            };

            var parkingSpotImageList = new List<Domain.Entities.ParkingSpotImage>
            {
                new Domain.Entities.ParkingSpotImage
                {
                    ParkingSpotImageId = 1,
                    ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                    ParkingId = 5
                },

            };
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);
            _parkingSpotImageRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.ParkingSpotImage, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync(parkingSpotImageList);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_Return_Not_Found_When_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new GetListImageByParkingIdQuery
            {
                ParkingId = 5,
                PageNo = 1,
                PageSize = 10
            };

            var parkingSpotImageList = new List<Domain.Entities.ParkingSpotImage>
            {
                new Domain.Entities.ParkingSpotImage
                {
                    ParkingSpotImageId = 1,
                    ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                    ParkingId = 5
                },

            };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync((Domain.Entities.Parking)null);
            

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListImageByParkingIdQuery
            {
                ParkingId = 5,
                PageNo = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task Handle_WithNoParkingSpotImage_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListImageByParkingIdQuery
            {
                ParkingId = 5,
                PageNo = 1000,
                PageSize = 10
            };
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);
            _parkingSpotImageRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.ParkingSpotImage, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync((List<Domain.Entities.ParkingSpotImage>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}

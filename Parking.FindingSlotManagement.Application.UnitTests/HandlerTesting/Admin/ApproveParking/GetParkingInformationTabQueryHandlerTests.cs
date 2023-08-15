using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ApproveParking
{
    public class GetParkingInformationTabQueryHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly Mock<IParkingSpotImageRepository> _parkingSpotImageRepositoryMock;
        private readonly GetParkingInformationTabQueryHandler _handler;
        public GetParkingInformationTabQueryHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _parkingSpotImageRepositoryMock = new Mock<IParkingSpotImageRepository>();
            _handler = new GetParkingInformationTabQueryHandler(_parkingRepositoryMock.Object, _fieldWorkParkingImgRepositoryMock.Object,_timeSlotRepositoryMock.Object, _parkingSlotRepositoryMock.Object, _parkingHasPriceRepositoryMock.Object, _parkingSpotImageRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenParkingDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var parkingId = 1;
            _parkingRepositoryMock.Setup(r => r.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true))
                .ReturnsAsync((Domain.Entities.Parking)null);


            var query = new GetParkingInformationTabQuery { ParkingId = parkingId };

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
            response.Data.ShouldBeNull();
        }
    }
}

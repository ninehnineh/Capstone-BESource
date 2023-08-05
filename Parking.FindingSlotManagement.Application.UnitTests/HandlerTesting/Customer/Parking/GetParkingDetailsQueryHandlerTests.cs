/*using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Parking
{
    public class GetParkingDetailsQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly GetParkingDetailsQueryHandler _handler;
        public GetParkingDetailsQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetParkingDetailsQueryHandler(_parkingRepositoryMock.Object, _mapperMock.Object, _parkingHasPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Success_Response_When_Parking_Exists()
        {
            // Arrange
            GetParkingDetailsQuery x = new GetParkingDetailsQuery()
            {
                ParkingId = 1
            };

            var cancellationToken = new CancellationToken();

            // Create the test data
            var parking = new Domain.Entities.Parking { ParkingId = 1, IsActive = true, 
                ParkingHasPrices = new List<ParkingHasPrice> { new ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = 1 } }, 
                ParkingSpotImages = new List<ParkingSpotImage> { new ParkingSpotImage {ParkingSpotImageId = 1, ParkingId = 1 } } };

            // Set up the mock repository methods to return the test data
            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), true))
                .ReturnsAsync(parking);
            List<ParkingHasPrice> parkingHasPrices = new List<ParkingHasPrice>
            {
                new ParkingHasPrice
                {
                     Parking = new Domain.Entities.Parking
                    {
                        ParkingId = 1,
                        IsActive = true
                    },
                     ParkingId = 1,
                     ParkingPriceId = 1,
                    ParkingPrice = new ParkingPrice
                    {
                        ParkingPriceId = 1,
                        IsActive = true,
                        Traffic = new Traffic
                        {
                            TrafficId = 1
                        },
                        TimeLines = new List<TimeLine> { new TimeLine { TimeLineId = 1, ParkingPriceId = 1,IsActive = true } }
                    },
                }

            };
            _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(), null, true)).ReturnsAsync(parkingHasPrices);

            // Act
            var result = await _handler.Handle(x, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            
        }
    }
}
*/
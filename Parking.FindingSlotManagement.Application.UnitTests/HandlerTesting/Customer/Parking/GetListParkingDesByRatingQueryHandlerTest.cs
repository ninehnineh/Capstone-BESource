/*using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating;
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
    public class GetListParkingDesByRatingQueryHandlerTest
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<ITimelineRepository> _timelineRepositoryMock;
        private readonly GetListParkingDesByRatingQueryHandler _handler;
        public GetListParkingDesByRatingQueryHandlerTest()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _timelineRepositoryMock = new Mock<ITimelineRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetListParkingDesByRatingQueryHandler(_parkingRepositoryMock.Object, _parkingHasPriceRepositoryMock.Object, _parkingPriceRepositoryMock.Object, _timelineRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenParkingFound_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new GetListParkingDesByRatingQuery
            {
                // Set the properties of your request object as needed for the test
                PageNo = 1,
                PageSize = 10
            };
            var cancellationToken = CancellationToken.None;


            // Set up the mock responses for your dependencies
            var parkingList = new List<Domain.Entities.Parking>
            {
                // Add some Parking entities for testing
                new Domain.Entities.Parking { ParkingId = 1, IsActive = true, IsAvailable = true, Stars = 4, Latitude = 10.0M, Longitude = 20.0M },
                // Add more entities if needed for your test scenarios
            };
            var parkingHasPriceList = new List<ParkingHasPrice>
            {
                // Add some ParkingHasPrice entities for testing
                new ParkingHasPrice { ParkingPriceId = 1, ParkingId = 1},
                // Add more entities if needed for your test scenarios
            };
            var parkingPrice = new ParkingPrice
            {
                // Set properties of ParkingPrice for testing
                ParkingPriceId = 1,
                TrafficId = 1,
            };
            var timelineList = new List<TimeLine>
            {
                // Add some TimeLine entities for testing
                new TimeLine { ParkingPriceId = 1, StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(18), Price = 10 },
                // Add more entities if needed for your test scenarios
            };

            // Set up the mock responses for your mapper
            var expectedParkingShowInCusDto = new ParkingShowInCusDto
            {
                ParkingId = 1
            };
            var expectedResponse = new GetListParkingDesByRatingResponse
            {
                ParkingShowInCusDto = expectedParkingShowInCusDto,

            };

            // Set up the mocks to return the appropriate data when called
            _parkingRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Parking, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )).ReturnsAsync(parkingList);

            _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(
                It.IsAny<Expression<Func<ParkingHasPrice, bool>>>()
            )).ReturnsAsync(parkingHasPriceList);

            _parkingPriceRepositoryMock.Setup(repo => repo.GetById(
                It.IsAny<int>()
            )).ReturnsAsync(parkingPrice);

            _timelineRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(
                It.IsAny<Expression<Func<TimeLine, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.TimeLine, object>>>>(),
                It.IsAny<Expression<Func<Domain.Entities.TimeLine, int>>>(),
                It.IsAny<bool>()
            )).ReturnsAsync(timelineList);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<ParkingShowInCusDto>(
                It.IsAny<Domain.Entities.Parking>()
            )).Returns(expectedParkingShowInCusDto);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
        }
    }
}
*/
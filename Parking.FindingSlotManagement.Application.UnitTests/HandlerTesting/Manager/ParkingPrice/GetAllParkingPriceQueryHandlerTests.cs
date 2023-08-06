using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class GetAllParkingPriceQueryHandlerTests
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly GetAllParkingPriceQueryHandler _handler;
        public GetAllParkingPriceQueryHandlerTests()
        {
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _mapperMock = new Mock<IMapper>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _handler = new GetAllParkingPriceQueryHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object, _businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingBusinessProfileAndParkingPrices_ShouldReturnResponseWithData()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId


            var businessProfile = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId = 1,
                UserId = 1, User = new Domain.Entities.User { UserId = 1}
            };

            var parkingPrices = new List<Domain.Entities.ParkingPrice>
            {
                new Domain.Entities.ParkingPrice
                {
                    ParkingPriceId = 1
                },
                new Domain.Entities.ParkingPrice
                {
                    ParkingPriceId = 2
                }
                // Add more parkingPrice records if needed
            };

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(),
                null, true
            )).ReturnsAsync(businessProfile);

            _parkingPriceRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                null,
                It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )).ReturnsAsync(parkingPrices);

            var query = new GetAllParkingPriceQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_NonExistingBusinessProfile_ShouldReturnEmptyResponse()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId

            var mockParkingPriceRepository = new Mock<IParkingPriceRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();

            mockBusinessProfileRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(),
                null, true
            )).ReturnsAsync((Domain.Entities.BusinessProfile)null);

            var query = new GetAllParkingPriceQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy");
        }
        [Fact]
        public async Task Handle_NoParkingPricesFound_ShouldReturnEmptyResponse()
        {
            // Arrange
            int managerId = 1; // Replace with a valid managerId

            var mockParkingPriceRepository = new Mock<IParkingPriceRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();

            var businessProfile = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId= 1,
            };

            mockBusinessProfileRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(),
                null, true
            )).ReturnsAsync(businessProfile);

            mockParkingPriceRepository.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                null,
                It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )).ReturnsAsync(new List<Domain.Entities.ParkingPrice>());

            var query = new GetAllParkingPriceQuery
            {
                ManagerId = managerId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy");
        }
    }
}

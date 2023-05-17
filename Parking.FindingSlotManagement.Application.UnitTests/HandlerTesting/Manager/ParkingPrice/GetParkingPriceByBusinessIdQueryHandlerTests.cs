using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class GetParkingPriceByBusinessIdQueryHandlerTests
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetParkingPriceByBusinessIdQueryHandlerTests()
        {
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Hanlde_Should_ReturnEmptyResult_WhenBusinessHasNoParkingPrice()
        {
            var request = new GetAllParkingPriceQuery { BusinessId = 1, PageNo = 1, PageSize = 1};

            var data = new List<Domain.Entities.ParkingPrice>();

            _parkingPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                    It.IsAny<List<Expression<Func<Domain.Entities.ParkingPrice, object>>>>(),
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )).ReturnsAsync(data);

            var handler = new GetAllParkingPriceQueryHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, default);

            result.Data.ShouldBeNull();
            result.Message.ShouldBe("Không tìm thấy");
            result.StatusCode.ShouldBe(200);
            result.Success.ShouldBeTrue();

        }
        
        [Fact]
        public async Task Hanlde_Should_ReturnEmptyResult_WhenHasParkingPrice_ButNotOfSpecificBusiness()
        {
            var request = new GetAllParkingPriceQuery { BusinessId = 1, PageNo = 1, PageSize = 1};

            var data = new List<Domain.Entities.ParkingPrice> { 
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 1,
                    ParkingPriceName = "Test",
                    UserId = 2,
                }
            };

            _parkingPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                    It.IsAny<List<Expression<Func<Domain.Entities.ParkingPrice, object>>>>(),
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )).ReturnsAsync(data.Where(x => x.UserId == request.BusinessId));

            var handler = new GetAllParkingPriceQueryHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Data.ShouldBeNull();
            result.Message.ShouldBe("Không tìm thấy");
            result.StatusCode.ShouldBe(200);
            result.Success.ShouldBeTrue();

        }
        
        [Fact]
        public async Task Hanlde_Should_ReturnOneResult_WhenHasParkingPrice_ButOnlyForSpecificBusiness()
        {
            var request = new GetAllParkingPriceQuery { BusinessId = 1, PageNo = 1, PageSize = 1};

            var data = new List<Domain.Entities.ParkingPrice> { 
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 1,
                    ParkingPriceName = "Test",
                    UserId = 2,
                },
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 2,
                    ParkingPriceName = "Test",
                    UserId = 1,
                }
            };

            _parkingPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                    It.IsAny<List<Expression<Func<Domain.Entities.ParkingPrice, object>>>>(),
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )).ReturnsAsync(data.Where(x => x.UserId == request.BusinessId).Take(request.PageSize));

            var handler = new GetAllParkingPriceQueryHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Success.ShouldBeTrue();
        }
        
        [Fact]
        public async Task Hanlde_Should_ReturnOneResult_WhenParkingPriceHasTwo_ButOnlyForSpecificBusiness_AndPageSizeIsOne()
        {
            var request = new GetAllParkingPriceQuery { BusinessId = 1, PageNo = 1, PageSize = 1};

            var data = new List<Domain.Entities.ParkingPrice> { 
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 1,
                    ParkingPriceName = "Test",
                    UserId = 2,
                },
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 2,
                    ParkingPriceName = "Test",
                    UserId = 1,
                },
                new Domain.Entities.ParkingPrice
                {
                    IsActive = true,
                    ParkingPriceId = 3,
                    ParkingPriceName = "Test",
                    UserId = 1,
                }
            };

            _parkingPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, bool>>>(),
                    It.IsAny<List<Expression<Func<Domain.Entities.ParkingPrice, object>>>>(),
                    It.IsAny<Expression<Func<Domain.Entities.ParkingPrice, int>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )).ReturnsAsync(data.Where(x => x.UserId == request.BusinessId).Take(request.PageSize));

            var handler = new GetAllParkingPriceQueryHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Success.ShouldBeTrue();
        }
    }
}

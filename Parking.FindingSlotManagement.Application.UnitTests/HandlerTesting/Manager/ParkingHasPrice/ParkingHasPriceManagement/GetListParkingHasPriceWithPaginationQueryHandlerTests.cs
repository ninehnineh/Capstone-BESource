using AutoMapper;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement;

public class GetListParkingHasPriceWithPaginationQueryHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
    private readonly GetListParkingHasPriceWithPaginationQueryHandler _handler;

    public GetListParkingHasPriceWithPaginationQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
        _handler = new GetListParkingHasPriceWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task Handle_ValidParkingHasPriceList_ShouldReturnResponseWithItems()
    {
        // Arrange
        int parkingId = 1; // Replace with a valid parking ID

        var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();
        var mockMapper = new Mock<IMapper>();

        var parkingHasPriceList = new List<Domain.Entities.ParkingHasPrice>
        {
            new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = 1 },
            new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 2, ParkingId = 1  },
            new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 3, ParkingId = 1  }
        };

        mockParkingHasPriceRepository.Setup(repo => repo.GetAllItemWithPagination(
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
            It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        )).ReturnsAsync(parkingHasPriceList);

        var handler = new GetListParkingHasPriceWithPaginationQueryHandler(
            mockParkingHasPriceRepository.Object,
            mockMapper.Object
        );
        var query = new GetListParkingHasPriceWithPaginationQuery
        {
            ParkingId = parkingId,
            PageNo = 1,
            PageSize = 10
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.StatusCode.ShouldBe(200);
        result.Data.ShouldNotBeNull();
        result.Count.ShouldBe(parkingHasPriceList.Count);
        result.Message.ShouldBe("Thành công");
    }
    [Fact]
    public async Task Handle_EmptyParkingHasPriceList_ShouldReturnEmptyResponse()
    {
        // Arrange
        int parkingId = 1; // Replace with a valid parking ID


        _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
            It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        )).ReturnsAsync(new List<Domain.Entities.ParkingHasPrice>());

        var query = new GetListParkingHasPriceWithPaginationQuery
        {
            ParkingId = parkingId,
            PageNo = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Success.ShouldBeTrue();
        result.StatusCode.ShouldBe(200);
        result.Count.ShouldBe(0);
        result.Message.ShouldBe("Không tim thấy");
    }
    [Fact]
    public async Task Handle_ExceptionThrown_ShouldThrowException()
    {
        // Arrange
        int parkingId = 1; // Replace with a valid parking ID


        _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithPagination(
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
            It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        )).Throws(new Exception("Simulated exception"));

        var query = new GetListParkingHasPriceWithPaginationQuery
        {
            ParkingId = parkingId,
            PageNo = 1,
            PageSize = 10
        };

        // Act & Assert
        await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
        // You can also check the specific exception message if needed.
    }
}


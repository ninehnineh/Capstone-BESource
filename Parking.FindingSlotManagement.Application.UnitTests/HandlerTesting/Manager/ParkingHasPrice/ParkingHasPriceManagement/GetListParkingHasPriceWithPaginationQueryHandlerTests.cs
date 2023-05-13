//using AutoMapper;
//using Moq;
//using Org.BouncyCastle.Asn1.Ocsp;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Drawing.Printing;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement;

//public class GetListParkingHasPriceWithPaginationQueryHandlerTests
//{
//    private readonly Mock<IMapper> _mapperMock;
//    private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;

//    public GetListParkingHasPriceWithPaginationQueryHandlerTests()
//    {
//        _mapperMock = new Mock<IMapper>();
//        _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
//    }

//    [Fact]
//    public async Task Handle_Returns_Success_Response_With_Count_Zero_When_ListParkingHasPrice_Is_Empty()
//    {
//        // Arrange
//        GetListParkingHasPriceWithPaginationQuery request = MediatorRequest();

//        var listParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>();

//        _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
//            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
//            It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
//            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
//            It.IsAny<bool>(),
//            It.IsAny<int>(),
//            It.IsAny<int>()
//        )).ReturnsAsync(listParkingHasPrice);

//        var expectedResponse = new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
//        {
//            Success = true,
//            StatusCode = 200,
//            Message = "Không tim thấy",
//            Count = 0
//        };

//        var service = new GetListParkingHasPriceWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object , _mapperMock.Object);

//        // Act
//        var result = await service.Handle(request, CancellationToken.None);

//        // Assert
//        result.Success.ShouldBeTrue();
//        result.StatusCode.ShouldBe(200);
//        result.Message.ShouldBe("Không tim thấy");
//        result.Count.ShouldBe(0);
//    }

//    [Fact]
//    public async Task Handle_ReturnsSuccessResponseWithData_When_ListParkingHasPriceIsNotEmpty_And_HasOneRecord()
//    {
//        // Arrange
//        GetListParkingHasPriceWithPaginationQuery request = MediatorRequest();

//        List<Domain.Entities.ParkingHasPrice> listParkingHasPrice = ListParkingHasPriceWithOneRecord();

//        GetRecondFromExitedListWithCondition(listParkingHasPrice);

//        var expectedResponse = new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
//        {
//            Data = new List<GetListParkingHasPriceWithPaginationResponse>
//            {
//                new GetListParkingHasPriceWithPaginationResponse
//                {
//                    ParkingHasPriceId = 1,
//                    ParkingName = "Parking 1",
//                    ParkingPrice = new Domain.Entities.TimeLine
//                    {
//                        PackagePriceId = 1,
//                        Price = 10000,
//                    }
//                }
//            },
//            Success = true,
//            StatusCode = 200,
//            Message = "Thành công",
//            Count = 1
//        };

//        _mapperMock.Setup(x => x.Map<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>(listParkingHasPrice))
//            .Returns(expectedResponse.Data);

//        var service = new GetListParkingHasPriceWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object, _mapperMock.Object);

//        // Act
//        var result = await service.Handle(request, CancellationToken.None);

//        // Assert
//        result.Success.ShouldBeTrue();
//        result.StatusCode.ShouldBe(200);
//        result.Message.ShouldBe("Thành công");
//        result.Count.ShouldBe(1);
//    }
    
//    [Fact]
//    public async Task Handle_ReturnsSuccessResponseWithData_When_ListParkingHasPriceIsNotEmpty_And_HasMultipleRecord()
//    {
//        // Arrange
//        GetListParkingHasPriceWithPaginationQuery request = MediatorRequest();

//        List<Domain.Entities.ParkingHasPrice> listParkingHasPrice = ListParkingHasPriceWithTwoRecord();

//        GetRecondFromExitedListWithCondition(listParkingHasPrice);

//        var expectedResponse = new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
//        {
//            Data = new List<GetListParkingHasPriceWithPaginationResponse>
//            {
//                new GetListParkingHasPriceWithPaginationResponse
//                {
//                    ParkingHasPriceId = 1,
//                    ParkingName = "Parking 1",
//                    ParkingPrice = new Domain.Entities.TimeLine
//                    {
//                        PackagePriceId = 1,
//                        Price = 10000,
//                    }
//                },
//                new GetListParkingHasPriceWithPaginationResponse
//                {
//                    ParkingHasPriceId = 2,
//                    ParkingName = "Parking 2",
//                    ParkingPrice = new Domain.Entities.TimeLine
//                    {
//                        PackagePriceId = 2,
//                        Price = 10000,
//                    }
//                }
//            },
//            Success = true,
//            StatusCode = 200,
//            Message = "Thành công",
//            Count = 1
//        };

//        _mapperMock.Setup(x => x.Map<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>(listParkingHasPrice))
//            .Returns(expectedResponse.Data);

//        var service = new GetListParkingHasPriceWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object, _mapperMock.Object);

//        // Act
//        var result = await service.Handle(request, CancellationToken.None);

//        // Assert
//        result.Success.ShouldBeTrue();
//        result.StatusCode.ShouldBe(200);
//        result.Message.ShouldBe("Thành công");
//        result.Count.ShouldBe(2);
//    }

//    //[Fact]
//    //public async Task Handle_Returns_SuccessResponseWithData_When_ListParkingHasPriceIsNotEmpty_And_HasMultipleRecord_AndPaging()
//    //{
//    //    // Arrange
//    //    var request = new GetListParkingHasPriceWithPaginationQuery
//    //    {
//    //        ManagerId = 1,
//    //        PageNo = 1,
//    //        PageSize = 1
//    //    };

//    //    var listParkingHasPrice = new List<Domain.Entities.ParkingHasPrice>
//    //    {
//    //        new Domain.Entities.ParkingHasPrice
//    //        {
//    //            ParkingHasPriceId = 1,
//    //            ParkingId = 1,
//    //            Parking = new Domain.Entities.Parking { ManagerId = 1, Name = "Parking 1" },
//    //            ParkingPrice = new Domain.Entities.PackagePrice { PackagePriceId = 1, Price = 10000 }
//    //        },
//    //        new Domain.Entities.ParkingHasPrice
//    //        {
//    //            ParkingHasPriceId = 2,
//    //            Parking = new Domain.Entities.Parking { ManagerId = 2, Name = "Parking 2" },
//    //            ParkingPrice = new Domain.Entities.PackagePrice { PackagePriceId = 2, Price = 20000 }
//    //        }
//    //    };

//    //    var includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
//    //        {
//    //            x => x.Parking!,
//    //            y => y.ParkingPrice!
//    //        };

//    //    _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
//    //        It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
//    //        includes,
//    //        It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
//    //        It.IsAny<bool>(),
//    //        It.IsAny<int>(),
//    //        It.IsAny<int>()
//    //    )).ReturnsAsync(listParkingHasPrice);

//    //    var expectedResponse = new ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>
//    //    {
//    //        Data = new List<GetListParkingHasPriceWithPaginationResponse>
//    //        {
//    //            new GetListParkingHasPriceWithPaginationResponse
//    //            {
//    //                ParkingHasPriceId = 1,
//    //                ParkingName = "Parking 1",
//    //                ParkingPrice = new Domain.Entities.PackagePrice
//    //                {
//    //                    PackagePriceId = 1,
//    //                    Price = 10000,
//    //                }
//    //            },
//    //        },
//    //        Success = true,
//    //        StatusCode = 200,
//    //        Message = "Thành công",
//    //        Count = 1
//    //    };

//    //    _mapperMock.Setup(x => x.Map<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>(listParkingHasPrice))
//    //        .Returns(expectedResponse.Data);

//    //    var service = new GetListParkingHasPriceWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object, _mapperMock.Object);

//    //    // Act
//    //    var result = await service.Handle(request, CancellationToken.None);

//    //    // Assert
//    //    result.Success.ShouldBeTrue();
//    //    result.StatusCode.ShouldBe(200);
//    //    result.Message.ShouldBe("Thành công");
//    //    result.Count.ShouldBe(1);
//    //}

//    private void GetRecondFromExitedListWithCondition(List<Domain.Entities.ParkingHasPrice> listParkingHasPrice)
//    {
//        _parkingHasPriceRepositoryMock.Setup(x => x.GetAllItemWithPagination(
//            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
//            It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
//            It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
//            It.IsAny<bool>(),
//            It.IsAny<int>(),
//            It.IsAny<int>()
//        )).ReturnsAsync(listParkingHasPrice);
//    }

//    private static GetListParkingHasPriceWithPaginationQuery MediatorRequest()
//    {
//        return new GetListParkingHasPriceWithPaginationQuery
//        {
//            ManagerId = 1,
//            PageNo = 1,
//            PageSize = 10
//        };
//    }

//    private static List<Domain.Entities.ParkingHasPrice> ListParkingHasPriceWithOneRecord()
//    {
//        return new List<Domain.Entities.ParkingHasPrice>
//        {
//            new Domain.Entities.ParkingHasPrice
//            {
//                ParkingHasPriceId = 1,
//                Parking = new Domain.Entities.Parking
//                {
//                    ParkingId = 1,
//                    Name = "Parking 1",
//                    ManagerId = 1
//                },
//                ParkingPrice = new Domain.Entities.TimeLine
//                {
//                    PackagePriceId = 1,
//                    Price = 10000,
//                }
//            },
//        };
//    }
//    private static List<Domain.Entities.ParkingHasPrice> ListParkingHasPriceWithTwoRecord()
//    {
//        return new List<Domain.Entities.ParkingHasPrice>
//        {
//            new Domain.Entities.ParkingHasPrice
//            {
//                ParkingHasPriceId = 1,
//                ParkingId = 1,
//                ParkingPriceId = 1,
//                Parking = new Domain.Entities.Parking
//                {
//                    ParkingId = 1,
//                    Name = "Parking 1",
//                    ManagerId = 1
//                },
//                ParkingPrice = new Domain.Entities.TimeLine
//                {
//                    PackagePriceId = 1,
//                    Price = 10000,
//                }
//            },
            
//            new Domain.Entities.ParkingHasPrice
//            {
//                ParkingHasPriceId = 2,
//                ParkingId = 2,
//                ParkingPriceId = 2,
//                Parking = new Domain.Entities.Parking
//                {
//                    ParkingId = 2,
//                    Name = "Parking 2",
//                    ManagerId = 2
//                },
//                ParkingPrice = new Domain.Entities.TimeLine
//                {
//                    PackagePriceId = 2,
//                    Price = 10000,
//                }
//            },
//        };
//    }
//}


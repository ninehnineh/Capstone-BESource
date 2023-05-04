using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressByUserId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.FavoriteAddress.FavoriteAddressManagement
{
    public class GetFavoriteAddressByUserIdQueryHandlerTests
    {
        private readonly Mock<IFavoriteAddressRepository> _favoriteAddressRepositoryMock;
        private readonly GetFavoriteAddressByUserIdQueryHandler _handler;
        public GetFavoriteAddressByUserIdQueryHandlerTests()
        {
            _favoriteAddressRepositoryMock = new Mock<IFavoriteAddressRepository>();
            _handler = new GetFavoriteAddressByUserIdQueryHandler(_favoriteAddressRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetFavoriteAddressByUserIdQuery
            {
                UserId = 10,
                PageNo = 1,
                PageSize = 10
            };

            var traffics = new List<Domain.Entities.FavoriteAddress>
            {
                new Domain.Entities.FavoriteAddress
                {
                    FavoriteAddressId = 1,
                    TagName = "Nhà riêng",
                    Address = "Khu phức hợp Thảo Loan Plaza: Đường 9A, Khu dân cư Trung Sơn, Xã Bình Hưng, Huyện Bình Chánh, TPHCM.",
                    UserId = 10
                },

            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.FavoriteAddress, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync(traffics);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetFavoriteAddressByUserIdQuery
            {
                UserId = 10,
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
        public async Task Handle_WithNoTRafficFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetFavoriteAddressByUserIdQuery
            {
                UserId = 10,
                PageNo = 1000,
                PageSize = 10
            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.FavoriteAddress, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync((List<Domain.Entities.FavoriteAddress>)null);

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

using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.FavoriteAddress.FavoriteAddressManagement
{
    public class GetFavoriteAddressByIdQueryHandlerTests
    {
        private readonly Mock<IFavoriteAddressRepository> _favoriteAddressRepositoryMock;
        private readonly GetFavoriteAddressByIdQueryHandler _handler;
        public GetFavoriteAddressByIdQueryHandlerTests()
        {
            _favoriteAddressRepositoryMock = new Mock<IFavoriteAddressRepository>();
            _handler = new GetFavoriteAddressByIdQueryHandler(_favoriteAddressRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetFavoriteAddressByIdQuery()
            {
                FavoriteAddressId = 1
            };
            var traffic = new Domain.Entities.FavoriteAddress
            {
                FavoriteAddressId = 1,
                TagName = "Nhà riêng",
                Address = "Khu phức hợp Thảo Loan Plaza: Đường 9A, Khu dân cư Trung Sơn, Xã Bình Hưng, Huyện Bình Chánh, TPHCM.",
                UserId = 10
            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(query.FavoriteAddressId)).ReturnsAsync(traffic);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidId_ReturnsNotFound()
        {
            var query = new GetFavoriteAddressByIdQuery()
            {
                FavoriteAddressId = 9999999
            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(query.FavoriteAddressId)).ReturnsAsync((Domain.Entities.FavoriteAddress)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}

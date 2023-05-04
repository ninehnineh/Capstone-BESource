using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.DeleteFavoriteAddress;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.FavoriteAddress.FavoriteAddressManagement
{
    public class DeleteFavoriteAddressCommandHandlerTests
    {
        private readonly Mock<IFavoriteAddressRepository> _favoriteAddressRepositoryMock;
        private readonly DeleteFavoriteAddressCommandHandler _handler;
        public DeleteFavoriteAddressCommandHandlerTests()
        {
            _favoriteAddressRepositoryMock = new Mock<IFavoriteAddressRepository>();
            _handler = new DeleteFavoriteAddressCommandHandler(_favoriteAddressRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_DeleteFavoriteAddress_When_Exist()
        {
            // Arrange
            var command = new DeleteFavoriteAddressCommand { FavoriteAddressId = 1 };
            var favoriteAddress = new Domain.Entities.FavoriteAddress 
            { 
                FavoriteAddressId = command.FavoriteAddressId,
                TagName = "Nhà riêng",
                Address = "Khu phức hợp Thảo Loan Plaza: Đường 9A, Khu dân cư Trung Sơn, Xã Bình Hưng, Huyện Bình Chánh, TPHCM.",
                UserId = 10

            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(command.FavoriteAddressId)).ReturnsAsync(favoriteAddress);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _favoriteAddressRepositoryMock.Verify(x => x.Delete(favoriteAddress), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeleteFavoriteAddressCommand { FavoriteAddressId = 1 };
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(command.FavoriteAddressId)).ReturnsAsync((Domain.Entities.FavoriteAddress)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");

            _favoriteAddressRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.FavoriteAddress>()), Times.Never);
        }
    }
}

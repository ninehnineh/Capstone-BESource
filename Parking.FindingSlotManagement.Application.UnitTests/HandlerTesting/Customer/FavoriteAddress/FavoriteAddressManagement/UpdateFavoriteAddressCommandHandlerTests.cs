using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.UpdateFavoriteAddress;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.FavoriteAddress.FavoriteAddressManagement
{
    public class UpdateFavoriteAddressCommandHandlerTests
    {
        private readonly Mock<IFavoriteAddressRepository> _favoriteAddressRepositoryMock;
        private readonly UpdateFavoriteAddressCommandValidation _validator;
        private readonly UpdateFavoriteAddressCommandHandler _handler;

        public UpdateFavoriteAddressCommandHandlerTests()
        {
            _favoriteAddressRepositoryMock = new Mock<IFavoriteAddressRepository>();
            _validator = new UpdateFavoriteAddressCommandValidation();
            _handler = new UpdateFavoriteAddressCommandHandler(_favoriteAddressRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateFavoriteAddressCommandHandler_Should_Update_FavoriteAddress_Successfully()
        {
            // Arrange
            var request = new UpdateFavoriteAddressCommand
            {
                FavoriteAddressId = 1,
                TagName = "Công ty",
                Address = "125/83 Luong The Vinh Street, Tan Thoi Hoa Ward"
            };
            var cancellationToken = new CancellationToken();
            var Oldtraffic = new Domain.Entities.FavoriteAddress
            {
                FavoriteAddressId = 1,
                TagName = "Nhà riêng",
                Address = "Khu phức hợp Thảo Loan Plaza: Đường 9A, Khu dân cư Trung Sơn, Xã Bình Hưng, Huyện Bình Chánh, TPHCM.",
            };
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(request.FavoriteAddressId))
                .ReturnsAsync(Oldtraffic);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            Oldtraffic.TagName.ShouldBe(request.TagName);
            Oldtraffic.Address.ShouldBe(request.Address);
            _favoriteAddressRepositoryMock.Verify(x => x.Update(Oldtraffic), Times.Once);
        }
        [Fact]
        public async Task UpdateFavoriteAddressCommandHandler_Should_Return_Not_Found_If_FavoriteAddress_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateFavoriteAddressCommand
            {
                FavoriteAddressId = 2000
            };
            var cancellationToken = new CancellationToken();
            _favoriteAddressRepositoryMock.Setup(x => x.GetById(request.FavoriteAddressId))
                .ReturnsAsync((Domain.Entities.FavoriteAddress)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _favoriteAddressRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.FavoriteAddress>()), Times.Never);
        }
        [Fact]
        public void TagName_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateFavoriteAddressCommand
            {
                FavoriteAddressId = 1,
                TagName = "lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagName);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateFavoriteAddressCommand
            {
                FavoriteAddressId = 1,
                Address = "lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
    }
}

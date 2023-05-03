using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.FavoriteAddress.FavoriteAddressManagement
{
    public class CreateNewFavoriteAddressCommandHandlerTests
    {
        private readonly Mock<IFavoriteAddressRepository> _favoriteAddressRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly CreateNewFavoriteAddressCommandHandler _handler;
        private readonly CreateNewFavoriteAddressCommandValidation _validator;
        public CreateNewFavoriteAddressCommandHandlerTests()
        {
            _favoriteAddressRepositoryMock = new Mock<IFavoriteAddressRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new CreateNewFavoriteAddressCommandHandler(_favoriteAddressRepositoryMock.Object, _accountRepositoryMock.Object);
            _validator = new CreateNewFavoriteAddressCommandValidation();        
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewFavoriteAddressCommand 
            {
                TagName = "Văn phòng",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };
            /*var expectedFavoriteAddress = new Domain.Entities.FavoriteAddress 
            {
                FavoriteAddressId = 2,
                TagName = "Văn phòng",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };*/
            var checkUserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkUserExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _favoriteAddressRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.FavoriteAddress>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidUserId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "Văn phòng",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 2
            };

            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe(200);

            _favoriteAddressRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.FavoriteAddress>()), Times.Never);
        }
        [Fact]
        public void TagName_ShouldNotBeEmpty()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagName);
        }
        [Fact]
        public void TagName_ShouldNotBeNull()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagName);
        }
        [Fact]
        public void TagName_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TagName);
        }
        [Fact]
        public void Address_ShouldNotBeEmpty()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "Văn phòng",
                Address = "",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotBeNull()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "Văn phòng",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewFavoriteAddressCommand
            {
                TagName = "Văn phòng",
                Address = "Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000 Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
    }
}

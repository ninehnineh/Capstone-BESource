using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Wallet
{
    public class GetWalletByUserIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetWalletByUserIdQueryHandler _queryHandler;

        public GetWalletByUserIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetWalletByUserIdQueryHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object
            );
        }
        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetWalletByUserIdQuery { UserId = userId };

            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true)).ReturnsAsync((User)null);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.Data.ShouldBeNull();
            _userRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true), Times.Once);
            _mapperMock.Verify(x => x.Map<GetWalletByUserIdResponse>(It.IsAny<Domain.Entities.Wallet>()), Times.Never);
        }
        [Fact]
        public async Task Handle_UserFound_ReturnsValidResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetWalletByUserIdQuery { UserId = userId };
            var user = new User { UserId = userId, Wallet = new Domain.Entities.Wallet { WalletId = 1 } };
            var expectedResponse = new ServiceResponse<GetWalletByUserIdResponse>
            {
                Data = new GetWalletByUserIdResponse { WalletId = 1 },
                Success = true,
                StatusCode = 200,
                Message = "Thành công"
            };

            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<GetWalletByUserIdResponse>(It.IsAny<Domain.Entities.Wallet>())).Returns(expectedResponse.Data);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Data.ShouldBe(expectedResponse.Data);
            _userRepositoryMock.Verify(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true), Times.Once);
            _mapperMock.Verify(x => x.Map<GetWalletByUserIdResponse>(It.IsAny<Domain.Entities.Wallet>()), Times.Once);
        }
    }
}

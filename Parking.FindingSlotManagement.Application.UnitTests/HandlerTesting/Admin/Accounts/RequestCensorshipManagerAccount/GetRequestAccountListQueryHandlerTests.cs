using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.RequestCensorshipManagerAccount
{
    public class GetRequestAccountListQueryHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly GetRequestAccountListQueryHandler _handler;
        public GetRequestAccountListQueryHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new GetRequestAccountListQueryHandler(_accountRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetRequestAccountListQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var accounts = new List<User>
            {
                new User { UserId = 10, Name = "Linh Đỗ Anh", Email= "linhdase151281@fpt.edu.vn", Phone = "0325995900", Avatar = "https://haycafe.vn/wp-content/uploads/2022/03/avatar-facebook-doc.jpg", DateOfBirth = DateTime.Parse("2000-12-11"), IsCensorship = false, IsActive = true },
                new User { UserId = 7, Name = "Chính Trương Công", Email= "chinh.truongcong202@gmail.com", Phone = "0560560566", Avatar = "https://kiemtientuweb.com/ckfinder/userfiles/images/avatar-fb/avatar-fb-1.jpg", DateOfBirth = DateTime.Parse("1998-04-26"), IsCensorship = false, IsActive = true },
                new User { UserId = 6, Name = "le dat", Email= "jike25062001@gmail.com", Phone = "0111111111", Avatar = "https://i0.wp.com/thatnhucuocsong.com.vn/wp-content/uploads/2023/02/Hinh-anh-avatar-Facebook.jpg?ssl=1", DateOfBirth = DateTime.Parse("2000-04-26"), IsCensorship = false, IsActive = true },
            };
            _accountRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<User, bool>>>(), null, x => x.UserId, true, request.PageNo, request.PageSize)).ReturnsAsync(accounts);

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
            var request = new GetRequestAccountListQuery
            {
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
        public async Task Handle_WithNoAccountFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetRequestAccountListQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var accounts = new List<User>();
            _accountRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<User, bool>>>(), null, x => x.UserId, true, request.PageNo, request.PageSize)).ReturnsAsync(accounts);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy");
        }
    }
}

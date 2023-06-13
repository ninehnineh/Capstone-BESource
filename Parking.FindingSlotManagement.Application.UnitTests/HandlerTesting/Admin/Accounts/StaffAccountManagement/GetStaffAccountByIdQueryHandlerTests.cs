using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetStaffAccountById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.StaffAccountManagement
{
    public class GetStaffAccountByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetStaffAccountByIdQueryHandler _handler;
        public GetStaffAccountByIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetStaffAccountByIdQueryHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetStaffAccountByIdQuery()
            {
                UserId = 1
            };
            var user = new Domain.Entities.User
            {
                UserId = 1,
                Name = "Linh Đỗ Anh",
                Email = "linhdase151281@fpt.edu.vn",
                Phone = "0325995900",
                Avatar = "https://haycafe.vn/wp-content/uploads/2022/03/avatar-facebook-doc.jpg",
                DateOfBirth = DateTime.Parse("2000-12-11"),
                IsCensorship = true,
                IsActive = true,
                RoleId = 4
            };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), true)).ReturnsAsync(user);

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
            var query = new GetStaffAccountByIdQuery()
            {
                UserId = 9999999
            };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), true)).ReturnsAsync((Domain.Entities.User)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.ShouldBeNull<GetStaffAccountByIdResponse>();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
        [Fact]
        public async Task Handle_WithInvalidId_ReturnsNotFound_When_Role_Is_Not_A_Staff()
        {
            var query = new GetStaffAccountByIdQuery()
            {
                UserId = 1
            };
            var user = new Domain.Entities.User
            {
                UserId = 1,
                Name = "Linh Đỗ Anh",
                Email = "linhdase151281@fpt.edu.vn",
                Phone = "0325995900",
                Avatar = "https://haycafe.vn/wp-content/uploads/2022/03/avatar-facebook-doc.jpg",
                DateOfBirth = DateTime.Parse("2000-12-11"),
                IsCensorship = true,
                IsActive = true,
                RoleId = 3
            };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), true)).ReturnsAsync(user);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.ShouldBeNull<GetStaffAccountByIdResponse>();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tài khoản không phải staff.");
        }
    }
}

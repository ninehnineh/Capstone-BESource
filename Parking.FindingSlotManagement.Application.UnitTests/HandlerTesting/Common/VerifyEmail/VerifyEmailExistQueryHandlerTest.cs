using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.VerifyEmail.Queries.VerifyEmailExist;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.VerifyEmail
{
    public class VerifyEmailExistQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly VerifyEmailExistQueryHandler _handler;
        public VerifyEmailExistQueryHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new VerifyEmailExistQueryHandler(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenEmailDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new VerifyEmailExistQuery { Email = "nonexistent@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), null, true))
                               .ReturnsAsync((Domain.Entities.User)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
        [Fact]
        public async Task Handle_WhenEmailExistsForUnauthorizedRoles_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var request = new VerifyEmailExistQuery { Email = "existing@example.com" };
            var userWithNonAuthorizedRole = new Domain.Entities.User { RoleId = 3, Email = "existing@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), null, true))
                               .ReturnsAsync(userWithNonAuthorizedRole);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Email không thuộc quyền truy cập hệ thống.");
        }
        [Fact]
        public async Task Handle_WhenEmailExistsForAuthorizedRoles_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new VerifyEmailExistQuery { Email = "existing@example.com" };
            var userWithAuthorizedRole = new Domain.Entities.User { RoleId = 1, Email = "existing@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), null, true))
                               .ReturnsAsync(userWithAuthorizedRole);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");
        }
    }
}

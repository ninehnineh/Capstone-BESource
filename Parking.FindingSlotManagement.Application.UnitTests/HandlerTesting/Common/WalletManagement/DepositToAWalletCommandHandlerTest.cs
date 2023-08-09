using Microsoft.Extensions.Configuration;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.WalletManagement.Commands.DepositToAWallet;
using Parking.FindingSlotManagement.Application.Models.Wallet;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.WalletManagement
{
    public class DepositToAWalletCommandHandlerTest
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _iConfigurationMock;
        private readonly Mock<IVnPayService> _vnPayServiceMock;
        private readonly DepositToAWalletCommandHandler _handler;
        public DepositToAWalletCommandHandlerTest()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _iConfigurationMock = new Mock<IConfiguration>();
            _vnPayServiceMock = new Mock<IVnPayService>();
            _handler = new DepositToAWalletCommandHandler(_walletRepositoryMock.Object, _userRepositoryMock.Object, _iConfigurationMock.Object, _vnPayServiceMock.Object);
        }
        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new DepositToAWalletCommand { UserId = 1, TotalPrice = 100 };
            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true))
                               .ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy tài khoản hoặc tài khoản đã bị hệ thống ban.");
            response.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_WhenUserIsNotAllowedToDeposit_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var request = new DepositToAWalletCommand { UserId = 1, TotalPrice = 100 };
            var user = new User { UserId = 1, RoleId = 4, IsActive = true }; // RoleId 4 means unauthorized user
            _userRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<Expression<Func<User, object>>>>(), true))
                               .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Tài khoản của bạn không được phép nạp tiền vào ví.");
            response.Data.ShouldBeNull();
        }
    }
}

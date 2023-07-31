/*using AutoMapper;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Commands.CustomerRegister;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Authentication
{
    public class CustomerRegisterCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly JwtSettings _jwtSettingsMock;
        private readonly CustomerRegisterCommandHandler _handler;
        public CustomerRegisterCommandHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _configurationMock = new Mock<IConfiguration>();
            IOptions<JwtSettings> options = Options.Create(_jwtSettingsMock);
            _handler = new CustomerRegisterCommandHandler(_userRepositoryMock.Object, _walletRepositoryMock.Object, options, _configurationMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsSuccessResponseWithToken()
        {
            // Arrange
            var request = new CustomerRegisterCommand
            {
                Phone = "0111111111",
                Address = "string",
                DateOfBirth = new DateTime(1930, 01, 06),
                Gender = "Nam",
                IdCardDate = new DateTime(1930, 01, 06),
                IdCardIssuedBy = "string",
                IdCardNo = "string",
                Name = "Gesture"
            };
            // Mock the behavior of the userRepository.GetItemWithCondition method to return a valid user.
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), true))
            .ReturnsAsync(new Domain.Entities.User { UserId = 1, RoleId = 3, Phone = "123456789" });

            // Mock the behavior of the userRepository.Insert method to return the inserted user.
            _userRepositoryMock.Setup(x => x.Insert(It.IsAny<Domain.Entities.User>()));

            // Mock the behavior of the walletRepository.Insert method to return the inserted wallet.
            _walletRepositoryMock.Setup(x => x.Insert(It.IsAny<Wallet>()));

            // Mock the token generation.
            var mockTokenValue = "mocked-token-value";
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("your-secret-key");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("your-issuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("your-audience");
            var token = new TokenManage(_jwtSettingsMock, _configurationMock.Object);
            var generateTokenMethod = token.GetType().GetMethod("GenerateToken", BindingFlags.NonPublic | BindingFlags.Instance);
            var generatedToken = generateTokenMethod.Invoke(token, new object[] { new Domain.Entities.User { UserId = 1, RoleId = 3 } }) as string;
            generatedToken.ShouldBe(mockTokenValue);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(201);
            result.Data.ShouldBe(mockTokenValue);
        }
    }
}
*/
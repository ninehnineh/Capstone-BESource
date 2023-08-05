using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetBanCountByUserId;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Account
{
    public class GetBanCountByUserIdQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetBanCountByUserIdQueryHandler _handler;
        public GetBanCountByUserIdQueryHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetBanCountByUserIdQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_WhenUserExists_ShouldReturnSuccessResponse()
        {
            // Arrange
            var userId = 123; 


            // Set up the mock to return a user entity when GetById is called
            var expectedUserEntity = new User
            {
                // Initialize with appropriate user data
                UserId = userId,
                BanCount = 0
                // ...
            };
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(expectedUserEntity);

            // Set up the mapper mock to return a response object
            var expectedResponse = new GetBanCountByUserIdResponse
            {
                // Initialize with appropriate data
                BanCount = 0,
                // ...
            };
            _mapperMock.Setup(mapper => mapper.Map<GetBanCountByUserIdResponse>(expectedUserEntity)).Returns(expectedResponse);

            // Create the instance of the class you are testing (your service) and pass the mocks to it


            // Act
            var request = new GetBanCountByUserIdQuery { UserId = userId };
            var cancellationToken = CancellationToken.None;
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldBe(expectedResponse);
        }
        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var userId = 456; // Replace with a user ID that does not exist in the system

            // Set up the mock to return null when GetById is called (user does not exist)
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);


            // Act
            var request = new GetBanCountByUserIdQuery { UserId = userId };
            var cancellationToken = CancellationToken.None;
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy người dùng.");
            result.Data.ShouldBeNull();
        }
    }
}

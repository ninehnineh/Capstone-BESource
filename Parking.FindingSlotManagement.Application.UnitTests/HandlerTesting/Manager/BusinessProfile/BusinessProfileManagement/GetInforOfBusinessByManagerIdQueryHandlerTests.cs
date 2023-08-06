using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetInforOfBusinessByManagerId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.BusinessProfile.BusinessProfileManagement
{
    public class GetInforOfBusinessByManagerIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IMapper> mapperMock;

        // Instance of the handler
        private readonly GetInforOfBusinessByManagerIdQueryHandler handler;

        public GetInforOfBusinessByManagerIdQueryHandlerTests()
        {
            // Initialize mocked dependencies and the handler
            userRepositoryMock = new Mock<IUserRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new GetInforOfBusinessByManagerIdQueryHandler(userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidManagerId_ShouldReturnInforOfBusiness()
        {
            // Arrange
            var managerId = 1; // Replace with a valid manager ID
            var managerEntity = new User
            {
                UserId = managerId,
                RoleId = 1, // Assuming 1 is the ID for the manager role
                Role = new Role { RoleId = 1, Name = "Manager" },
                BusinessProfile = new Domain.Entities.BusinessProfile { BusinessProfileId = 1, UserId = 1 }
            };

            // Setup the mock for the GetUserById method
            userRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync(managerEntity);

            // Setup the mapper to return a mapped response
            mapperMock.Setup(mapper => mapper.Map<GetInforOfBusinessByManagerIdResponse>(managerEntity))
                .Returns(new GetInforOfBusinessByManagerIdResponse { BusinessProfileId = 1});

            var query = new GetInforOfBusinessByManagerIdQuery { ManagerId = managerId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            userRepositoryMock.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);

            // Assert the response
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            // Add more specific assertions for the response data if needed
        }
        [Fact]
        public async Task Handle_InvalidManagerId_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var managerId = 999; // Replace with an invalid manager ID
            userRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync((User)null); // Return null for an invalid manager ID

            var query = new GetInforOfBusinessByManagerIdQuery { ManagerId = managerId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            userRepositoryMock.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);

            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
        [Fact]
        public async Task Handle_NonManagerRole_ShouldReturnForbiddenResponse()
        {
            // Arrange
            var managerId = 1; // Replace with a valid manager ID
            var nonManagerEntity = new User
            {
                UserId = managerId,
                RoleId = 2, // Assuming 2 is the ID for a non-manager role
                Role = new Role { RoleId = 2, Name = "NonManager" },
                BusinessProfile = new Domain.Entities.BusinessProfile { BusinessProfileId = 1, UserId = 1 }
            };

            userRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()))
                .ReturnsAsync(nonManagerEntity);

            var query = new GetInforOfBusinessByManagerIdQuery { ManagerId = managerId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            userRepositoryMock.Verify(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()), Times.Once);

            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe("Tài khoản không phải là quản lý của doanh nghiệp.");
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldReturnErrorResponse()
        {
            // Arrange
            var managerId = 1; // Replace with a valid manager ID
            userRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<List<Expression<Func<User, object>>>>(),
                It.IsAny<bool>()))
                .Throws(new Exception("Simulated exception"));

            var query = new GetInforOfBusinessByManagerIdQuery { ManagerId = managerId };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}

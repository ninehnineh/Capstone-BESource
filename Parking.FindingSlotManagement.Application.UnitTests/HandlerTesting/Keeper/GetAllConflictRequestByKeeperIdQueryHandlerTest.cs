using AutoMapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Keeper
{
    public class GetAllConflictRequestByKeeperIdQueryHandlerTest
    {
        private readonly Mock<IConflictRequestRepository> _conflictRequestRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetAllConflictRequestByKeeperIdQueryHandler _handler;
        public GetAllConflictRequestByKeeperIdQueryHandlerTest()
        {
            _conflictRequestRepositoryMock = new Mock<IConflictRequestRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetAllConflictRequestByKeeperIdQueryHandler(_conflictRequestRepositoryMock.Object, _mapperMock.Object, _userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidKeeperId_ReturnsConflictRequests()
        {
            // Arrange
            int keeperId = 1;
            int parkingId = 1;
            var request = new GetAllConflictRequestByKeeperIdQuery
            {
                KeeperId = keeperId,
                PageNo = 1,
                PageSize = 10
            };

            // Create mock repositories
            var mockConflictRequestRepository = new Mock<IConflictRequestRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Setup mock user repository to return a keeper with the specified parking ID
            var keeperUser = new User { UserId = keeperId, RoleId = 2, ParkingId = parkingId };
            mockUserRepository.Setup(repo => repo.GetById(keeperId))
                .ReturnsAsync(keeperUser);

            // Setup mock conflict request repository to return conflict requests for the parking ID
            var conflictRequests = new List<ConflictRequest>
        {
            new ConflictRequest { ConflictRequestId = 1, ParkingId = parkingId },
            new ConflictRequest { ConflictRequestId = 2, ParkingId = parkingId },
            // Add more conflict requests as needed...
        };
            mockConflictRequestRepository.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(),
                null,
                It.IsAny<Expression<Func<ConflictRequest, int>>>(),
                It.IsAny<bool>(),
                request.PageNo,
                request.PageSize
            ))
            .ReturnsAsync(conflictRequests);

            // Setup mock mapper to return a mapped response for each conflict request
            var mappedResponses = conflictRequests.Select(conflictRequest => new GetAllConflictRequestByKeeperIdResponse
            {
                ConflictRequestId = conflictRequest.ConflictRequestId,
                // Map other properties as needed...
            });
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(conflictRequests))
                .Returns(mappedResponses);

            // Create an instance of the query handler
            var handler = new GetAllConflictRequestByKeeperIdQueryHandler(
                mockConflictRequestRepository.Object,
                mockMapper.Object,
                mockUserRepository.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeEmpty();
            result.Count.ShouldBe(conflictRequests.Count);

            // Verify that the repositories' methods were called
            mockUserRepository.Verify(repo => repo.GetById(keeperId), Times.Once);
            mockConflictRequestRepository.Verify(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(),
                null,
                It.IsAny<Expression<Func<ConflictRequest, int>>>(),
                It.IsAny<bool>(),
                request.PageNo,
                request.PageSize
            ), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(conflictRequests), Times.Once);
        }
        [Fact]
        public async Task Handle_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            int keeperId = 1;
            var request = new GetAllConflictRequestByKeeperIdQuery
            {
                KeeperId = keeperId,
                PageNo = 1,
                PageSize = 10
            };

            // Create mock repositories
            var mockConflictRequestRepository = new Mock<IConflictRequestRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Setup mock user repository to return null (user not found)
            mockUserRepository.Setup(repo => repo.GetById(keeperId))
                .ReturnsAsync((User)null);

            // Create an instance of the query handler
            var handler = new GetAllConflictRequestByKeeperIdQueryHandler(
                mockConflictRequestRepository.Object,
                mockMapper.Object,
                mockUserRepository.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);

            // Verify that the user repository's method was called
            mockUserRepository.Verify(repo => repo.GetById(keeperId), Times.Once);
            mockConflictRequestRepository.Verify(repo => repo.GetAllItemWithPagination(It.IsAny<Expression<Func<ConflictRequest, bool>>>(), null, It.IsAny<Expression<Func<ConflictRequest, int>>>(), It.IsAny<bool>(), request.PageNo, request.PageSize), Times.Never);
            mockMapper.Verify(mapper => mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(It.IsAny<IEnumerable<ConflictRequest>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_UserNotKeeper_ReturnsInvalidUserRoleResponse()
        {
            // Arrange
            int keeperId = 1;
            var request = new GetAllConflictRequestByKeeperIdQuery
            {
                KeeperId = keeperId,
                PageNo = 1,
                PageSize = 10
            };

            // Create mock repositories
            var mockConflictRequestRepository = new Mock<IConflictRequestRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Setup mock user repository to return a user with a different role (not a keeper)
            var user = new User { UserId = keeperId, RoleId = 3, ParkingId = 1 };
            mockUserRepository.Setup(repo => repo.GetById(keeperId))
                .ReturnsAsync(user);

            // Create an instance of the query handler
            var handler = new GetAllConflictRequestByKeeperIdQueryHandler(
                mockConflictRequestRepository.Object,
                mockMapper.Object,
                mockUserRepository.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Tài khoản của bạn không phải bảo vệ của bãi.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);

            // Verify that the user repository's method was called
            mockUserRepository.Verify(repo => repo.GetById(keeperId), Times.Once);
            mockConflictRequestRepository.Verify(repo => repo.GetAllItemWithPagination(It.IsAny<Expression<Func<ConflictRequest, bool>>>(), null, It.IsAny<Expression<Func<ConflictRequest, int>>>(), It.IsAny<bool>(), request.PageNo, request.PageSize), Times.Never);
            mockMapper.Verify(mapper => mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(It.IsAny<IEnumerable<ConflictRequest>>()), Times.Never);
        }
        [Fact]
        public async Task Handle_NoConflictRequests_ReturnsEmptyResponse()
        {
            // Arrange
            int keeperId = 1;
            int parkingId = 1;
            var request = new GetAllConflictRequestByKeeperIdQuery
            {
                KeeperId = keeperId,
                PageNo = 1,
                PageSize = 10
            };

            // Create mock repositories
            var mockConflictRequestRepository = new Mock<IConflictRequestRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Setup mock user repository to return a keeper with the specified parking ID
            var keeperUser = new User { UserId = keeperId, RoleId = 2, ParkingId = parkingId };
            mockUserRepository.Setup(repo => repo.GetById(keeperId))
                .ReturnsAsync(keeperUser);

            // Setup mock conflict request repository to return an empty list
            var conflictRequests = new List<ConflictRequest>();
            mockConflictRequestRepository.Setup(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(),
                null,
                It.IsAny<Expression<Func<ConflictRequest, int>>>(),
                It.IsAny<bool>(),
                request.PageNo,
                request.PageSize
            ))
            .ReturnsAsync(conflictRequests);

            // Create an instance of the query handler
            var handler = new GetAllConflictRequestByKeeperIdQueryHandler(
                mockConflictRequestRepository.Object,
                mockMapper.Object,
                mockUserRepository.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy.");
            result.Count.ShouldBe(0);

            // Verify that the repositories' methods were called
            mockUserRepository.Verify(repo => repo.GetById(keeperId), Times.Once);
            mockConflictRequestRepository.Verify(repo => repo.GetAllItemWithPagination(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(),
                null,
                It.IsAny< Expression<Func<ConflictRequest, int>>>(),
                It.IsAny<bool>(),
                request.PageNo,
                request.PageSize
            ), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(It.IsAny<IEnumerable<ConflictRequest>>()), Times.Never);
        }
    }
}

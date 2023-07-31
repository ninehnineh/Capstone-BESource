using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.GetAllCustomer
{
    public class GetCustomerByIdQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly GetCustomerByIdQueryHandler _handler;
        public GetCustomerByIdQueryHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetCustomerByIdQueryHandler(_mockUserRepository.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Success_Response_When_User_Exists()
        {
            // Arrange
            var request = new GetCustomerByIdQuery { UserId = 1 };
            var cancellationToken = new CancellationToken();

            // Mocking User and Mapper
            var user = new User { UserId = 1, Name = "TestUser", RoleId = 2 };
            _mockUserRepository
                .Setup(x => x.GetItemWithCondition(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<List<Expression<Func<User, object>>>>(), true
                ))
                .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<GetCustomerByIdResponse>(user))
                .Returns(new GetCustomerByIdResponse { UserId = 1, Name = "TestUser" });


            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_User_Does_Not_Exists()
        {
            // Arrange
            var request = new GetCustomerByIdQuery { UserId = 1 };
            var cancellationToken = new CancellationToken();

            // Mocking User and Mapper
            _mockUserRepository
                .Setup(x => x.GetItemWithCondition(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<List<Expression<Func<User, object>>>>(), true
                ))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Không tìm thấy thông tin tài khoản.");
            result.StatusCode.ShouldBe(404);
        }
        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Repository_Throws_Exception()
        {
            // Arrange
            var request = new GetCustomerByIdQuery { UserId = 1 };
            var cancellationToken = new CancellationToken();

            // Mocking User Repository

            _mockUserRepository
                .Setup(x => x.GetItemWithCondition(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<List<Expression<Func<User, object>>>>(), true
                ))
                .ThrowsAsync(new Exception("Simulated repository exception."));

            var service = new GetCustomerByIdQueryHandler(_mockUserRepository.Object, null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Handle(request, cancellationToken));
        }
        
    }
}

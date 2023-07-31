using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetListCustomer;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
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
    public class GetListCustomerQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly GetListCustomerQueryHandler _handler;
        public GetListCustomerQueryHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetListCustomerQueryHandler(_mockUserRepository.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListCustomerQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var user = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Name = "Test"
                },

            };
            _mockUserRepository.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), x => x.UserId, true, request.PageNo, request.PageSize)).ReturnsAsync(user);

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
            var request = new GetListCustomerQuery
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
        public async Task Handle_WithNoPaypalInfoFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListCustomerQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var user = new List<User>();
            _mockUserRepository.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), x => x.UserId, true, request.PageNo, request.PageSize)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Danh sách trống.");
        }
    }
}

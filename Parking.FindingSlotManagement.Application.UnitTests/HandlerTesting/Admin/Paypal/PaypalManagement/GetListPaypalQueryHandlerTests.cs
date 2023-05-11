using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Paypal.PaypalManagement
{
    public class GetListPaypalQueryHandlerTests
    {
        private readonly Mock<IPaypalRepository> _paypalRepositoryMock;
        private readonly GetListPaypalQueryHandler _handler;
        public GetListPaypalQueryHandlerTests()
        {
            _paypalRepositoryMock = new Mock<IPaypalRepository>();
            _handler = new GetListPaypalQueryHandler(_paypalRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListPaypalQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var payPals = new List<PayPal>
            {
                new PayPal 
                { 
                    PayPalId = 1,
                    ClientId = "string",
                    SecretKey = "string",
                    ManagerId = 5
                },
               
            };
            _paypalRepositoryMock.Setup(x => x.GetAllItemWithPagination(null, It.IsAny<List<Expression<Func<Domain.Entities.PayPal, object>>>>(), x => x.PayPalId, true, request.PageNo, request.PageSize)).ReturnsAsync(payPals);

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
            var request = new GetListPaypalQuery
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
            var request = new GetListPaypalQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var payPals = new List<PayPal>();
            _paypalRepositoryMock.Setup(x => x.GetAllItemWithPagination(null, It.IsAny<List<Expression<Func<Domain.Entities.PayPal, object>>>>(), x => x.PayPalId, true, request.PageNo, request.PageSize)).ReturnsAsync(payPals);

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

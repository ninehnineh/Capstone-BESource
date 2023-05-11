using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId;
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
    public class GetPaypalByManagerIdQueryHandlerTest
    {
        private readonly Mock<IPaypalRepository> _paypalRepositoryMock;
        private readonly GetPaypalByManagerIdQueryHandler _handler;
        public GetPaypalByManagerIdQueryHandlerTest()
        {
            _paypalRepositoryMock = new Mock<IPaypalRepository>();
            _handler = new GetPaypalByManagerIdQueryHandler(_paypalRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidManagerId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetPaypalByManagerIdQuery()
            {
                ManagerId = 1
            };
            var payPal = new Domain.Entities.PayPal
            {
                ManagerId = 1
            };
            _paypalRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.PayPal, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.PayPal, object>>>>(), true)).ReturnsAsync(payPal);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidManagerId_ReturnsNotFound()
        {
            var query = new GetPaypalByManagerIdQuery()
            {
                ManagerId = 999999
            };
            _paypalRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.PayPal, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.PayPal, object>>>>(), true)).ReturnsAsync((Domain.Entities.PayPal)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy paypal.");
        }
    }
}

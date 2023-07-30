using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetFeeById;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Fee
{
    public class GetFeeByIdQueryHandlerTest
    {
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly GetFeeByIdQueryHandler _handler;
        public GetFeeByIdQueryHandlerTest()
        {
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _handler = new GetFeeByIdQueryHandler(_feeRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenFeeDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new GetFeeByIdQuery { FeeId = 1 };
            _feeRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Domain.Entities.Fee)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy.");
        }
        [Fact]
        public async Task Handle_WhenFeeExists_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetFeeByIdQuery { FeeId = 1 };
            var fee = new Domain.Entities.Fee { FeeId = 1, Name = "uuuuu" };

            _feeRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(fee);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");
            response.Data.ShouldNotBeNull();
        }
    }
}

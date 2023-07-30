using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetFeeById;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetListFee;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Fee
{
    public class GetListFeeQueryHandlerTest
    {
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly GetListFeeQueryHandler _handler;
        public GetListFeeQueryHandlerTest()
        {
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _handler = new GetListFeeQueryHandler(_feeRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenNoFeesExist_ReturnsSuccessResponseWithEmptyData()
        {
            // Arrange
            var request = new GetListFeeQuery();
            var emptyList = new List<Domain.Entities.Fee>(); // An empty list of fees
            _feeRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(null, null, x => x.FeeId, true))
                              .ReturnsAsync(emptyList);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy.");
        }
        [Fact]
        public async Task Handle_WhenFeesExist_ReturnsSuccessResponseWithData()
        {
            // Arrange
            var request = new GetListFeeQuery();
            var feeList = new List<Domain.Entities.Fee>
        {
            new Domain.Entities.Fee { FeeId = 1},
            new Domain.Entities.Fee { FeeId = 2},
            // Add more Fee objects as needed for testing
        };
            _feeRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(null, null, x => x.FeeId, true))
                              .ReturnsAsync(feeList);


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

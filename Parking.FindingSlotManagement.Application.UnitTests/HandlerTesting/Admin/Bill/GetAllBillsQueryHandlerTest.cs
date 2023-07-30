using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest;
using Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Bill
{
    public class GetAllBillsQueryHandlerTest
    {
        private readonly Mock<IBillRepository> _billRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly GetAllBillsQueryHandler _handler;
        public GetAllBillsQueryHandlerTest()
        {
            _billRepositoryMock = new Mock<IBillRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetAllBillsQueryHandler(_billRepositoryMock.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetAllBillsQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var bills = new List<Domain.Entities.Bill>
            {
                new Domain.Entities.Bill { BillId = 1},
                new Domain.Entities.Bill {BillId = 2},
                new Domain.Entities.Bill {BillId = 3},
            };
            _billRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Bill, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Bill, object>>>>(), x => x.BillId, true, request.PageNo, request.PageSize)).ReturnsAsync(bills);

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
            var request = new GetAllBillsQuery
            {
                PageNo = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task Handle_WithNoAccountFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetAllBillsQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var billss = new List<Domain.Entities.Bill>();
            _billRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Bill, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Bill, object>>>>(), x => x.BillId, true, request.PageNo, request.PageSize)).ReturnsAsync(billss);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}

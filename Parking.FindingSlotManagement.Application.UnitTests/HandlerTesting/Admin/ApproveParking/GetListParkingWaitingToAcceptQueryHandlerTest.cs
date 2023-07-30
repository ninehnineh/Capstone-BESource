using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetListCustomer;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetListParkingWaitingToAccept;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ApproveParking
{
    public class GetListParkingWaitingToAcceptQueryHandlerTest
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly GetListParkingWaitingToAcceptQueryHandler _handler;
        public GetListParkingWaitingToAcceptQueryHandlerTest()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetListParkingWaitingToAcceptQueryHandler(_approveParkingRepositoryMock.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListParkingWaitingToAcceptQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var approveParkings = new List<Domain.Entities.ApproveParking>
            {
                new Domain.Entities.ApproveParking
                {
                    ApproveParkingId = 1,
                    ParkingId = 1
                },

            };
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync(approveParkings);

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
            var request = new GetListParkingWaitingToAcceptQuery
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
            var request = new GetListParkingWaitingToAcceptQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var approveParkings = new List<Domain.Entities.ApproveParking>();
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), x => x.ApproveParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync(approveParkings);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}

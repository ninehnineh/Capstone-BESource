using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId;
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
    public class GetFieldInforByParkingIdQueryHandlerTests
    {
        private readonly Mock<IApproveParkingRepository> _approveParkingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IFieldWorkParkingImgRepository> _fieldWorkParkingImgRepositoryMock;
        private readonly GetFieldInforByParkingIdQueryHandler _handler;
        public GetFieldInforByParkingIdQueryHandlerTests()
        {
            _approveParkingRepositoryMock = new Mock<IApproveParkingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _fieldWorkParkingImgRepositoryMock = new Mock<IFieldWorkParkingImgRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetFieldInforByParkingIdQueryHandler(_parkingRepositoryMock.Object, _mapper.Object, _approveParkingRepositoryMock.Object, _fieldWorkParkingImgRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenParkingDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new GetFieldInforByParkingIdQuery { ParkingId = 1 };
            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
        }
        [Fact]
        public async Task Handle_WhenApproveParkingIsEmpty_ReturnsBadRequestResponse()
        {
            // Arrange
            var request = new GetFieldInforByParkingIdQuery { ParkingId = 1 };
            var parking = new Domain.Entities.Parking { ParkingId = 1 };
            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(parking);
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), null, true)).ReturnsAsync(new List<Domain.Entities.ApproveParking>());

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(400);
            response.Message.ShouldBe("Chưa có thông tin thực địa.");
        }
        [Fact]
        public async Task Handle_WhenApproveParkingIsNotEmpty_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetFieldInforByParkingIdQuery { ParkingId = 1 };
            var parking = new Domain.Entities.Parking { ParkingId = 1 };
            var approveParkingList = new List<Domain.Entities.ApproveParking>
        {
            new Domain.Entities.ApproveParking { ApproveParkingId = 1, ParkingId = 1, Status = "Đã_duyệt", StaffId = 101, User = new User { UserId = 101, Name = "John Doe" } },
            new Domain.Entities.ApproveParking { ApproveParkingId = 2, ParkingId = 1, Status = "Chờ_duyệt", StaffId = 102, User = new User { UserId = 102, Name = "Jane Smith" } },
        };

            _parkingRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(parking);
            _approveParkingRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.ApproveParking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.ApproveParking, object>>>>(), null, true)).ReturnsAsync(approveParkingList);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");
            response.Data.ShouldNotBeNull();
            response.Data.Count().ShouldBe(2);
            response.Data.ElementAt(0).StaffName.ShouldBe("John Doe");
            response.Data.ElementAt(1).StaffName.ShouldBe("Jane Smith");
        }
    }
}

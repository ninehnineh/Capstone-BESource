using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListParkingNewWNoApprove;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Staff.ApproveParking
{
    public class GetListParkingNewWNoApproveQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetListParkingNewWNoApproveQueryHandler _queryHandler;

        public GetListParkingNewWNoApproveQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _queryHandler = new GetListParkingNewWNoApproveQueryHandler(_parkingRepositoryMock.Object, GetMapper());
        }

        private IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); // Use your AutoMapper profile
            });
            return mapperConfig.CreateMapper();
        }
        [Fact]
        public async Task Handle_NoParking_ReturnsEmptyListResponse()
        {
            // Arrange
            var request = new GetListParkingNewWNoApproveQuery { PageNo = 1, PageSize = 10 };
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x => x.ParkingId, true, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Domain.Entities.Parking>());

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            _parkingRepositoryMock.Verify(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x => x.ParkingId, true, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ParkingWithNoApprove_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetListParkingNewWNoApproveQuery { PageNo = 1, PageSize = 10 };
            var parking1 = new Domain.Entities.Parking { ParkingId = 1, ApproveParkings = new List<Domain.Entities.ApproveParking>() };
            var parking2 = new Domain.Entities.Parking { ParkingId = 2, ApproveParkings = new List<Domain.Entities.ApproveParking>() };
            var parkingList = new List<Domain.Entities.Parking> { parking1, parking2 };
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x=> x.ParkingId, true, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(parkingList);

            // Act
            var result = await _queryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Data.ShouldContain(x => x.ParkingId == 1);
            result.Data.ShouldContain(x => x.ParkingId == 2);
            _parkingRepositoryMock.Verify(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x=> x.ParkingId, true, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}

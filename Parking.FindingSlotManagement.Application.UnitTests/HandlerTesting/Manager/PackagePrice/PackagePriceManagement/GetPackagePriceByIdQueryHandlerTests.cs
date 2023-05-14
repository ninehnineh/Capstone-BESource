//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Queries.GetPackagePriceById;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.PackagePrice.PackagePriceManagement
//{
//    public class GetPackagePriceByIdQueryHandlerTests
//    {
//        private readonly Mock<IPackagePriceRepository> _packagePriceRepositoryMock;
//        private readonly GetPackagePriceByIdQueryHandler _handler;
//        public GetPackagePriceByIdQueryHandlerTests()
//        {
//            _packagePriceRepositoryMock = new Mock<IPackagePriceRepository>();
//            _handler = new GetPackagePriceByIdQueryHandler(_packagePriceRepositoryMock.Object);
//        }
//        [Fact]
//        public async Task Handle_WithValidPackagePriceId_ReturnsSuccess()
//        {
//            // Arrange
//            var query = new GetPackagePriceByIdQuery()
//            {
//                PackagePriceId = 1
//            };
//            var packagePriceEntity = new Domain.Entities.TimeLine
//            {
//                PackagePriceId = 1,
//                Name = "Gói ngày cho xe ô tô",
//                Price = 20000,
//                Description = "Gói giữ xe ban ngày cho xe ô tô",
//                StartTime = DateTime.Now.Date.AddHours(5),
//                EndTime = DateTime.Now.Date.AddHours(17),
//                IsExtrafee = true,
//                ExtraFee = 5000,
//                ExtraTimeStep = 1,
//                HasPenaltyPrice = true,
//                PenaltyPrice = 20000,
//                PenaltyPriceStepTime = 1,
//                TrafficId = 1
//            };

//            _packagePriceRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.TimeLine, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.TimeLine, object>>>>(), true)).ReturnsAsync(packagePriceEntity);

//            // Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(200);
//            result.Message.ShouldBe("Thành công");
//        }
//        [Fact]
//        public async Task Handle_WithInvalidPackagePriceId_ReturnsNotFound()
//        {
//            var query = new GetPackagePriceByIdQuery()
//            {
//                PackagePriceId = 999999
//            };
//            _packagePriceRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.TimeLine, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.TimeLine, object>>>>(), true)).ReturnsAsync((Domain.Entities.TimeLine)null);
//            // Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(200);
//            result.Message.ShouldBe("Không tìm thấy gói.");
//        }
//    }
//}

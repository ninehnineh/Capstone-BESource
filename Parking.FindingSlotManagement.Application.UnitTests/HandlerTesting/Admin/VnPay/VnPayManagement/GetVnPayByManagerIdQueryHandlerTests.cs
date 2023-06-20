//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByManagerId;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.VnPay.VnPayManagement
//{
//    public class GetVnPayByManagerIdQueryHandlerTests
//    {
//        private readonly Mock<IVnPayRepository> _vnpayRepositoryMock;
//        private readonly GetVnPayByManagerIdQueryHandler _handler;
//        public GetVnPayByManagerIdQueryHandlerTests()
//        {
//            _vnpayRepositoryMock = new Mock<IVnPayRepository>();
//            _handler = new GetVnPayByManagerIdQueryHandler(_vnpayRepositoryMock.Object);
//        }
//        [Fact]
//        public async Task Handle_WithValidManagerId_ReturnsSuccess()
//        {
//            // Arrange
//            var query = new GetVnPayByManagerIdQuery()
//            {
//                ManagerId = 1
//            };
//            var vnpay = new Domain.Entities.VnPay
//            {
//                ManagerId = 1
//            };
//            _vnpayRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.VnPay, bool>>>(), It.IsAny < List<Expression<Func<Domain.Entities.VnPay, object>>>>(), true)).ReturnsAsync(vnpay);

//            // Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(200);
//            result.Message.ShouldBe("Thành công");
//        }
//        [Fact]
//        public async Task Handle_WithInvalidManagerId_ReturnsNotFound()
//        {
//            var query = new GetVnPayByManagerIdQuery()
//            {
//                ManagerId = 999999
//            };
//            _vnpayRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.VnPay, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.VnPay, object>>>>(), true)).ReturnsAsync((Domain.Entities.VnPay)null);
//            // Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            // Assert
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(200);
//            result.Message.ShouldBe("Không tìm thấy.");
//        }
//    }
//}

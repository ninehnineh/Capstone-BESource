using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.VnPay.VnPayManagement
{
    public class GetVnPayByIdQueryHandlerTest
    {
        private readonly Mock<IVnPayRepository> _vnpayRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly GetVnPayByIdQueryHandler _handler;
        public GetVnPayByIdQueryHandlerTest()
        {
            _vnpayRepositoryMock = new Mock<IVnPayRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetVnPayByIdQueryHandler(_vnpayRepositoryMock.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_WhenVnPayDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new GetVnPayByIdQuery { VnPayId = 1 };
            _vnpayRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Domain.Entities.VnPay)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(404);
            response.Message.ShouldBe("Không tìm thấy thông tin.");
        }
        [Fact]
        public async Task Handle_WhenVnPayExists_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetVnPayByIdQuery { VnPayId = 1 };
            var vnPay = new Domain.Entities.VnPay { VnPayId = 1, HashSecret = "uuuuu", TmnCode = "11222"};
            var responseDto = new GetVnPayByIdResponse { VnPayId = 1, HashSecret = "uuuuu" };

            _vnpayRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(vnPay);
            _mapper.Setup(x => x.Map<GetVnPayByIdResponse>(vnPay)).Returns(responseDto);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");
            response.Data.ShouldNotBeNull();
            response.Data.ShouldBeSameAs(responseDto);
        }
    }
}

using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.DeleteFee;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.DeleteVnPay;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Fee
{
    public class DeleteFeeCommandHandlerTest
    {
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly DeleteFeeCommandHandler _handler;
        public DeleteFeeCommandHandlerTest()
        {
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _handler = new DeleteFeeCommandHandler(_feeRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Fee_When_Exist()
        {
            // Arrange
            var command = new DeleteFeeCommand { FeeId = 1 };
            var feeExist = new Domain.Entities.Fee
            {
                FeeId = 1,
            };
            _feeRepositoryMock.Setup(x => x.GetById(command.FeeId)).ReturnsAsync(feeExist);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _feeRepositoryMock.Verify(x => x.Delete(feeExist), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeleteFeeCommand { FeeId = 1 };
            _feeRepositoryMock.Setup(x => x.GetById(command.FeeId)).ReturnsAsync((Domain.Entities.Fee)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");

            _feeRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.Fee>()), Times.Never);
        }
    }
}

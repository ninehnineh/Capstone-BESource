using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.DeleteVnPay;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.VnPay.VnPayManagement
{
    public class DeleteVnPayCommandHandlerTests
    {
        private readonly Mock<IVnPayRepository> _vnPayRepositoryMock;
        private readonly DeleteVnPayCommandHandler _handler;
        public DeleteVnPayCommandHandlerTests()
        {
            _vnPayRepositoryMock = new Mock<IVnPayRepository>();
            _handler = new DeleteVnPayCommandHandler(_vnPayRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_VnPayInfor_When_Exist()
        {
            // Arrange
            var command = new DeleteVnPayCommand { VnPayId = 1 };
            var vnpayInfor = new Domain.Entities.VnPay
            {
                VnPayId = 1,
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 5
            };
            _vnPayRepositoryMock.Setup(x => x.GetById(command.VnPayId)).ReturnsAsync(vnpayInfor);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _vnPayRepositoryMock.Verify(x => x.Delete(vnpayInfor), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeleteVnPayCommand { VnPayId = 1 };
            _vnPayRepositoryMock.Setup(x => x.GetById(command.VnPayId)).ReturnsAsync((Domain.Entities.VnPay)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");

            _vnPayRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.VnPay>()), Times.Never);
        }
    }
}

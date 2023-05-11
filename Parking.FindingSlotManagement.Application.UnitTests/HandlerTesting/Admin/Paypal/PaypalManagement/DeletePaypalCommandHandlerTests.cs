using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.DeletePaypal;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Paypal.PaypalManagement
{
    public class DeletePaypalCommandHandlerTests
    {
        private readonly Mock<IPaypalRepository> _paypalRepositoryMock;
        private readonly DeletePaypalCommandHandler _handler;
        public DeletePaypalCommandHandlerTests()
        {
            _paypalRepositoryMock = new Mock<IPaypalRepository>();
            _handler = new DeletePaypalCommandHandler(_paypalRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_PaypalInfor_When_Exist()
        {
            // Arrange
            var command = new DeletePaypalCommand { PayPalId = 1 };
            var paypalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };
            _paypalRepositoryMock.Setup(x => x.GetById(command.PayPalId)).ReturnsAsync(paypalInfor);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _paypalRepositoryMock.Verify(x => x.Delete(paypalInfor), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeletePaypalCommand { PayPalId = 1 };
            _paypalRepositoryMock.Setup(x => x.GetById(command.PayPalId)).ReturnsAsync((Domain.Entities.PayPal)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy paypal.");

            _paypalRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
    }
}

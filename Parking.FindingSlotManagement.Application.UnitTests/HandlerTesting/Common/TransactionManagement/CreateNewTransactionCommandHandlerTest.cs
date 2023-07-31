using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.TransactionManagement
{
    public class CreateNewTransactionCommandHandlerTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly CreateNewTransactionCommandHandler _handler;
        public CreateNewTransactionCommandHandlerTest()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new CreateNewTransactionCommandHandler(_transactionRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreateNewTransactionCommandIsValid_CreatesNewTransactionAndReturnsSuccessResponse()
        {
            // Arrange
            var request = new CreateNewTransactionCommand 
            { 
                Price = 20000M,
                Status = TransactionStatus.Nap_tien_vao_vi_thanh_cong.ToString(),
                WalletId = 1,
                Description = "string"
            };
            var entity = new Domain.Entities.Transaction 
            { 
                WalletId = 1,
                TransactionId = 1,
                Price = (decimal)request.Price
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(201);
            response.Message.ShouldBe("Nạp tiền thành công!!!");

            // Verify that the Insert method was called with the correct entity
            _transactionRepositoryMock.Verify(repo => repo.Insert(It.Is<Domain.Entities.Transaction>(t => t.Price == entity.Price)), Times.Once);
        }
    }
}

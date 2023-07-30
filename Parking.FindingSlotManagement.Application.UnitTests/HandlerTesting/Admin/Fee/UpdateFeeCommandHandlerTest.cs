using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.UpdateFee;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Fee
{
    public class UpdateFeeCommandHandlerTest
    {
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly UpdateFeeCommandHandler _handler;
        private readonly UpdateFeeCommandValidation _validator;
        public UpdateFeeCommandHandlerTest()
        {
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _validator = new UpdateFeeCommandValidation();
            _handler = new UpdateFeeCommandHandler(_feeRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateFeeCommandHandler_Should_Update_Fee_Successfully()
        {
            // Arrange
            var request = new UpdateFeeCommand
            {
                FeeId = 1,
                Name = "string",
                BusinessType = "string",
                Price = 4
            };
            var cancellationToken = new CancellationToken();
            var OldFee = new Domain.Entities.Fee
            {
                FeeId = 1,
                Name = "goi cho tu nhan",
                BusinessType = "goi cho tu nhan",
                Price = 5
            };

            _feeRepositoryMock.Setup(x => x.GetById(request.FeeId))
                .ReturnsAsync(OldFee);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldFee.Name.ShouldBe(request.Name);
            OldFee.BusinessType.ShouldBe(request.BusinessType);
            OldFee.Price.ShouldBe(request.Price);
            _feeRepositoryMock.Verify(x => x.Update(OldFee), Times.Once);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_payPalInfor_Does_Not_Exist()
        {

            var request = new UpdateFeeCommand
            {
                FeeId = 1,
                Name = "string",
                BusinessType = "string",
                Price = 4
            };
            var cancellationToken = new CancellationToken();
            _feeRepositoryMock.Setup(x => x.GetById(request.FeeId))
                .ReturnsAsync((Domain.Entities.Fee)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _feeRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Fee>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateFeeCommand
            {
                FeeId = 1,
                Name = "stringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstring",
                BusinessType = "string",
                Price = 4
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void BusinessType_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateFeeCommand
            {
                FeeId = 1,
                BusinessType = "stringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstring",
                Name = "string",
                Price = 4
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessType);
        }
        [Fact]
        public void Price_ShouldNotLessThanZero()
        {
            var command = new UpdateFeeCommand
            {
                FeeId = 1,
                BusinessType = "string",
                Name = "string",
                Price = -4
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
    }
}

using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.CreateNewFee;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Fee
{
    public class CreateNewFeeCommandHandlerTest
    {
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly CreateNewFeeCommandHandler _handler;
        private readonly CreateNewFeeCommandValidation _validator;
        public CreateNewFeeCommandHandlerTest()
        {
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _validator = new CreateNewFeeCommandValidation();
            _handler = new CreateNewFeeCommandHandler(_feeRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                BusinessType = "Goi cho tu nhan",
                Price = 5
            };
            var expectedFee = new Domain.Entities.Fee
            {
                Name = "Goi cho tu nhan",
                BusinessType = "Goi cho tu nhan",
                Price = 5
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            // Verify that the account repository was called to insert the new account
            _feeRepositoryMock.Verify(x => x.Insert(It.Is<Domain.Entities.Fee>(x => x.Name == expectedFee.Name)));
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "",
                BusinessType = "Goi cho tu nhan",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewFeeCommand
            {
                BusinessType = "Goi cho tu nhan",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhan",
                BusinessType = "Goi cho tu nhan",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void BusinessType_ShouldNotBeEmpty()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                BusinessType = "",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessType);
        }
        [Fact]
        public void BusinessType_ShouldNotBeNull()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessType);
        }
        [Fact]
        public void BusinessType_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                BusinessType = "Goi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoiGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoiGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoiGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi cho tu nhanGoi",
                Price = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessType);
        }
        [Fact]
        public void Price_ShouldNotBeNull()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                BusinessType = "Goi cho tu nhan",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
        [Fact]
        public void Price_ShouldNotLessThan0()
        {
            var command = new CreateNewFeeCommand
            {
                Name = "Goi cho tu nhan",
                BusinessType = "Goi cho tu nhan",
                Price = -5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
    }
}

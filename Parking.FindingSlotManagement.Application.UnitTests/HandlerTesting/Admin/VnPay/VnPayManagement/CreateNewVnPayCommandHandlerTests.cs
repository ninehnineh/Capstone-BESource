using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.VnPay.VnPayManagement
{
    public class CreateNewVnPayCommandHandlerTests
    {
        private readonly Mock<IVnPayRepository> _vnpayRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly CreateNewVnPayCommandValidation _validator;
        private readonly CreateNewVnPayCommandHandler _handler;
        public CreateNewVnPayCommandHandlerTests()
        {
            _vnpayRepositoryMock = new Mock<IVnPayRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _validator = new CreateNewVnPayCommandValidation();
            _handler = new CreateNewVnPayCommandHandler(_vnpayRepositoryMock.Object, _accountRepositoryMock.Object, _businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 1
            };
            var expectedVnPayInfor = new Domain.Entities.VnPay
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 1
            };
            var managerExist = new User { UserId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync(managerExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            // Verify that the account repository was called to insert the new account
            _vnpayRepositoryMock.Verify(x => x.Insert(It.Is<Domain.Entities.VnPay>(vnpay => vnpay.TmnCode == expectedVnPayInfor.TmnCode)));
        }
        [Fact]
        public async Task Handle_WhenManagerDoesNotExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 2
            };

            _accountRepositoryMock.Setup(x => x.GetById(request.UserId))
                .ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Không tìm thấy tài khoản.");

        }
        [Fact]
        public void TmnCode_ShouldNotBeEmpty()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TmnCode);
        }
        [Fact]
        public void TmnCode_ShouldNotBeNull()
        {
            var command = new CreateNewVnPayCommand
            {
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TmnCode);
        }
        [Fact]
        public void TmnCode_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZOLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TmnCode);
        }
        [Fact]
        public void HashSecret_ShouldNotBeEmpty()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "",
                UserId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.HashSecret);
        }
        [Fact]
        public void HashSecret_ShouldNotBeNull()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                UserId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.HashSecret);
        }
        [Fact]
        public void HashSecret_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFEVDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.HashSecret);
        }
        [Fact]
        public void UserId_ShouldNotBeEmpty()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
        [Fact]
        public void UserId_ShouldNotBeNull()
        {
            var command = new CreateNewVnPayCommand
            {
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}

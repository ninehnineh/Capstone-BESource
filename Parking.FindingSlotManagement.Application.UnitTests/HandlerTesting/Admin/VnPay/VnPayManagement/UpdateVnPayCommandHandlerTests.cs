using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.UpdateVnPay;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.VnPay.VnPayManagement
{
    public class UpdateVnPayCommandHandlerTests
    {
        private readonly Mock<IVnPayRepository> _vnpayRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly UpdateVnPayCommandValidation _validator;
        private readonly UpdateVnPayCommandHandler _handler;
        public UpdateVnPayCommandHandlerTests()
        {
            _vnpayRepositoryMock = new Mock<IVnPayRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new UpdateVnPayCommandValidation();
            _handler = new UpdateVnPayCommandHandler(_vnpayRepositoryMock.Object, _accountRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateVnPayCommandHandler_Should_Update_VnPayInfor_Successfully()
        {
            // Arrange
            var request = new UpdateVnPayCommand
            {
                VnPayId = 1,
                TmnCode = "string",
                HashSecret = "string",
                UserId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldVnPayInfor = new Domain.Entities.VnPay
            {
                VnPayId = 1,
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 5
            };

            _vnpayRepositoryMock.Setup(x => x.GetById(request.VnPayId))
                .ReturnsAsync(OldVnPayInfor);
            var checkManagerExist = new User { UserId = 5, IsActive = true, IsCensorship = true };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkManagerExist);
            _vnpayRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.UserId.Equals(checkManagerExist.UserId), null, true)).ReturnsAsync((Domain.Entities.VnPay)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldVnPayInfor.TmnCode.ShouldBe(request.TmnCode);
            OldVnPayInfor.HashSecret.ShouldBe(request.HashSecret);
            OldVnPayInfor.UserId.ShouldBe(request.UserId);
            _vnpayRepositoryMock.Verify(x => x.Update(OldVnPayInfor), Times.Once);
        }
        [Fact]
        public async Task UpdateVnPayCommandHandler_Should_Return_Not_Found_If_VnPayInfor_Does_Not_Exist()
        {

            var request = new UpdateVnPayCommand
            {
                VnPayId = 2000
            };
            var cancellationToken = new CancellationToken();
            _vnpayRepositoryMock.Setup(x => x.GetById(request.VnPayId))
                .ReturnsAsync((Domain.Entities.VnPay)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _vnpayRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.VnPay>()), Times.Never);
        }
        [Fact]
        public async Task UpdateVnPayCommandHandler_Should_Return_Not_Found_If_Manager_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateVnPayCommand
            {
                VnPayId = 1,
                TmnCode = "string",
                HashSecret = "string",
                UserId = 2
            };
            var cancellationToken = new CancellationToken();
            var OldVnPayInfor = new Domain.Entities.VnPay
            {
                VnPayId = 1,
                TmnCode = "OLJT0IRZ",
                HashSecret = "VDTZNUIMBXBBLPTPTNLXKHGQWWRCAGFE",
                UserId = 5
            };

            _vnpayRepositoryMock.Setup(x => x.GetById(request.VnPayId))
                .ReturnsAsync(OldVnPayInfor);
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _vnpayRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.VnPay>()), Times.Never);
        }
        [Fact]
        public void TmnCode_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateVnPayCommand
            {
                VnPayId = 1,
                TmnCode = "stringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstring",
                HashSecret = "string",
                UserId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TmnCode);
        }
        [Fact]
        public void HashSecret_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateVnPayCommand
            {
                VnPayId = 1,
                TmnCode = "string",
                HashSecret = "stringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstring",
                UserId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.HashSecret);
        }
    }
}

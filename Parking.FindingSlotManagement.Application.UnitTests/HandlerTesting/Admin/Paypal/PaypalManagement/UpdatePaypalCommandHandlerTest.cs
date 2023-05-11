using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Paypal.PaypalManagement
{
    public class UpdatePaypalCommandHandlerTest
    {
        private readonly Mock<IPaypalRepository> _paypalRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly UpdatePaypalCommandValidation _validator;
        private readonly UpdatePaypalCommandHandler _handler;
        public UpdatePaypalCommandHandlerTest()
        {
            _paypalRepositoryMock = new Mock<IPaypalRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new UpdatePaypalCommandValidation();
            _handler = new UpdatePaypalCommandHandler(_paypalRepositoryMock.Object, _accountRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Update_PayPalInfor_Successfully()
        {
            // Arrange
            var request = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "string",
                ManagerId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldPayPalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                .ReturnsAsync(OldPayPalInfor);
            var checkManagerExist = new User { UserId = 5, IsActive = true, IsCensorship = true, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(checkManagerExist.UserId)).ReturnsAsync(checkManagerExist);
            _paypalRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ManagerId.Equals(checkManagerExist.UserId), null, true)).ReturnsAsync((Domain.Entities.PayPal)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldPayPalInfor.ClientId.ShouldBe(request.ClientId);
            OldPayPalInfor.SecretKey.ShouldBe(request.SecretKey);
            OldPayPalInfor.ManagerId.ShouldBe(request.ManagerId);
            _paypalRepositoryMock.Verify(x => x.Update(OldPayPalInfor), Times.Once);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_payPalInfor_Does_Not_Exist()
        {

            var request = new UpdatePaypalCommand
            {
                PayPalId = 2000
            };
            var cancellationToken = new CancellationToken();
            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                .ReturnsAsync((Domain.Entities.PayPal)null);
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy paypal.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _paypalRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_Manager_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "string",
                ManagerId = 2
            };
            var cancellationToken = new CancellationToken();
            var OldPaypalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                .ReturnsAsync(OldPaypalInfor);
            _accountRepositoryMock.Setup(x => x.GetById(request.ManagerId)).ReturnsAsync((User)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tài khoản.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _paypalRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_Manager_Has_Banned()
        {
            // Arrange
            var request = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "string",
                ManagerId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldPaypalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                 .ReturnsAsync(OldPaypalInfor);
            var checkManagerExist = new User { UserId = 5, IsActive = false, IsCensorship = false, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(checkManagerExist.UserId)).ReturnsAsync(checkManagerExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _paypalRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_Manager_Has_Uncensorship()
        {
            // Arrange
            var request = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "string",
                ManagerId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldPaypalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                 .ReturnsAsync(OldPaypalInfor);
            var checkManagerExist = new User { UserId = 5, IsActive = true, IsCensorship = false, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(checkManagerExist.UserId)).ReturnsAsync(checkManagerExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _paypalRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
        [Fact]
        public async Task UpdatePaypalCommandHandler_Should_Return_Not_Found_If_Manager_Has_Already_Integrate_Paypal()
        {
            // Arrange
            var request = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "string",
                ManagerId = 2
            };
            var cancellationToken = new CancellationToken();
            var OldPaypalInfor = new Domain.Entities.PayPal
            {
                PayPalId = 1,
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _paypalRepositoryMock.Setup(x => x.GetById(request.PayPalId))
                .ReturnsAsync(OldPaypalInfor);
            var checkManagerExist = new User { UserId = 2, IsActive = true, IsCensorship = true, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(checkManagerExist.UserId)).ReturnsAsync(checkManagerExist);
            var managerAlreadyIntegratePaypal = new PayPal
            {
                PayPalId = 3,
                ClientId = "string2",
                SecretKey = "string2",
                ManagerId = 2
            };
            _paypalRepositoryMock.Setup(x => x.GetItemWithCondition(x => x.ManagerId.Equals(checkManagerExist.UserId), null, true)).ReturnsAsync(managerAlreadyIntegratePaypal);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _paypalRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.PayPal>()), Times.Never);
        }
        [Fact]
        public void ClientId_ShouldNotExceedMaximumLength()
        {
            var command = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "stringQuản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!",
                SecretKey = "string",
                ManagerId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ClientId);
        }
        [Fact]
        public void SecretKey_ShouldNotExceedMaximumLength()
        {
            var command = new UpdatePaypalCommand
            {
                PayPalId = 1,
                ClientId = "string",
                SecretKey = "stringQuản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!",
                ManagerId = 2
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.SecretKey);
        }
    }
}

using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Paypal.PaypalManagement
{
    public class CreateNewPaypalCommandHandlerTests
    {
        private readonly Mock<IPaypalRepository> _paypalRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly CreateNewPaypalCommandValidation _validator;
        private readonly CreateNewPaypalCommandHandler _handler;
        public CreateNewPaypalCommandHandlerTests()
        {
            _paypalRepositoryMock = new Mock<IPaypalRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new CreateNewPaypalCommandValidation();
            _handler = new CreateNewPaypalCommandHandler(_paypalRepositoryMock.Object, _accountRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };
            var expectedVnPayInfor = new Domain.Entities.PayPal
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };
            var managerExist = new User { UserId = 5, IsActive = true, IsCensorship = true, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(managerExist.UserId))
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
            _paypalRepositoryMock.Verify(x => x.Insert(It.Is<Domain.Entities.PayPal>(paypal => paypal.ClientId == expectedVnPayInfor.ClientId)));
        }
        [Fact]
        public async Task Handle_WhenManagerDoesNotExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            _accountRepositoryMock.Setup(x => x.GetById(request.ManagerId))
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
        public async Task Handle_WhenManagerHasBanned_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var managerExist = new User { UserId = 5, IsActive = false, IsCensorship = false, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(managerExist.UserId))
                .ReturnsAsync(managerExist);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.");

        }
        [Fact]
        public async Task Handle_WhenManagerHasUncensorship_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var managerExist = new User { UserId = 5, IsActive = true, IsCensorship = false, RoleId = 1 };
            _accountRepositoryMock.Setup(x => x.GetById(managerExist.UserId))
                .ReturnsAsync(managerExist);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.");

        }
        [Fact]
        public async Task Handle_When_RoleIsNotAManager_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var managerExist = new User { UserId = 5, IsActive = true, IsCensorship = true, RoleId = 2 };
            _accountRepositoryMock.Setup(x => x.GetById(managerExist.UserId))
                .ReturnsAsync(managerExist);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(200);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Tài khoản không phải là quản lý.");

        }
        [Fact]
        public void ClientId_ShouldNotBeEmpty()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ClientId);
        }
        [Fact]
        public void ClientId_ShouldNotBeNull()
        {
            var command = new CreateNewPaypalCommand
            {
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ClientId);
        }
        [Fact]
        public void ClientId_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuUAeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ClientId);
        }
        [Fact]
        public void SecretKey_ShouldNotBeEmpty()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.SecretKey);
        }
        [Fact]
        public void SecretKey_ShouldNotBeNull()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.SecretKey);
        }
        [Fact]
        public void SecretKey_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNpENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.SecretKey);
        }
        [Fact]
        public void ManagerId_ShouldNotBeEmpty()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
                ManagerId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ManagerId);
        }
        [Fact]
        public void ManagerId_ShouldNotBeNull()
        {
            var command = new CreateNewPaypalCommand
            {
                ClientId = "AeorWLhrPYGiCKXvq93kvcpItMykZqdSYmde2QYBJHAEomU_0gpo6oS93oDrZAc9OX5Wq9zF0NZipxuU",
                SecretKey = "ENMtcell7pD3JRMzRLo6H1q6VQplPQ1FhStAL4GaYNWECIeI716N4rnPJuDXfL7vpDN_ZYM9hBfA1kNp",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ManagerId);
        }
        
    }
}

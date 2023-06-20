//using FluentValidation.TestHelper;
//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.GenerateOTP;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Common.OTPManagement
//{
//    public class GenerateOTPCommandHandlerTests
//    {
//        private readonly Mock<IOTPRepository> _otpRepositoryMock;
//        private readonly Mock<IAccountRepository> _accountRepositoryMock;
//        private readonly Mock<IEmailService> _emailServiceMock;
//        private readonly GenerateOTPCommandValidation _validator;
//        private readonly GenerateOTPCommandHandler _handler;
//        public GenerateOTPCommandHandlerTests()
//        {
//            _otpRepositoryMock = new Mock<IOTPRepository>();
//            _accountRepositoryMock = new Mock<IAccountRepository>();
//            _emailServiceMock = new Mock<IEmailService>();
//            _validator = new GenerateOTPCommandValidation();
//            _handler = new GenerateOTPCommandHandler(_otpRepositoryMock.Object, _accountRepositoryMock.Object, _emailServiceMock.Object);
//        }
//        [Fact]
//        public async Task Handle_ShouldReturnSuccessResponse_WhenValidRequestIsProvided()
//        {
//            // Arrange
//            var email = "test@example.com";
//            var user = new User { UserId = 1, Email = email, IsActive = true, IsCensorship = true };
//            var otpExpirationTime = DateTime.UtcNow.AddHours(7).AddSeconds(60);
//            var otp = "123456";
//            var otpEntity = new OTP { Code = otp, ExpirationTime = otpExpirationTime, UserId = user.UserId };

//            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
//                                 .ReturnsAsync(user);

//            _otpRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<OTP, bool>>>(), null, false))
//                             .ReturnsAsync((OTP)null);
//            _otpRepositoryMock.Setup(x => x.Insert(It.IsAny<OTP>()));



//            var request = new GenerateOTPCommand { Email = email };

//            // Act
//            var result = await _handler.Handle(request, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(201);
//            result.Message.ShouldBe("Thành công");

//            _otpRepositoryMock.Verify(x => x.Insert(It.IsAny<OTP>()), Times.Once);
//        }
//        [Fact]
//        public async Task Handle_ShouldReturnErrorResponse_WhenUserNotFound()
//        {
//            // Arrange
//            var email = "test@example.com";

//            _accountRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<User, bool>>>(), null, true))
//                                 .ReturnsAsync((User)null);


//            var request = new GenerateOTPCommand { Email = email };

//            // Act
//            var result = await _handler.Handle(request, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.StatusCode.ShouldBe(200);
//            result.Message.ShouldBe("Không tìm thấy tài khoản hoặc tài khoản đã bị ban.");

//            _otpRepositoryMock.Verify(x => x.Insert(It.IsAny<OTP>()), Times.Never);
//        }
//        [Fact]
//        public void Email_ShouldNotBeEmpty()
//        {
//            var command = new GenerateOTPCommand
//            {
//                Email = ""
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Email);
//        }

//        [Fact]
//        public void Email_ShouldNotBeNull()
//        {
//            var command = new GenerateOTPCommand();

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Email);
//        }
//        [Fact]
//        public void Email_ShouldBeValidEmailAddress()
//        {
//            var command = new GenerateOTPCommand
//            {
//                Email = "notavalidemailaddress"
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Email);
//        }
//        [Fact]
//        public void Email_ShouldNotExceedMaximumLength()
//        {
//            var command = new GenerateOTPCommand
//            {
//                Email = "loremmfsadmfdmsfifjdsnmnksdakfgsaosfojdksfjsdfsadkfsdklafkljsdjkf.oldslfldsaflsdalfldsflsdfldslfldsfdsfnsdjffvsdafjdsvjk@gmail.com",
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.Email);
//        }
//    }
//}

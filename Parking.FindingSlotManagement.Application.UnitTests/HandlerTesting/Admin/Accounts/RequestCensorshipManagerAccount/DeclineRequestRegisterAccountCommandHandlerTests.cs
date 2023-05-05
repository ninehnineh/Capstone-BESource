using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.DeclineRequestRegisterAccount;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.RequestCensorshipManagerAccount
{
    public class DeclineRequestRegisterAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly DeclineRequestRegisterAccountCommandHandler _handler;
        private readonly Mock<IEmailService> _emailServiceMock;
        public DeclineRequestRegisterAccountCommandHandlerTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _handler = new DeclineRequestRegisterAccountCommandHandler(_mockAccountRepository.Object, _emailServiceMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ShouldReturnSuccessResponse()
        {
            // Arrange
            var userId = 3;
            var email = "ln.loinguyen.ln@gmail.com";
            var account = new User
            {
                UserId = userId,
                Name = "Lợi Nguyễn",
                Email = email,
                Phone = "0777777777",
                Avatar = "https://cdn-icons-png.flaticon.com/512/147/147144.png?w=360",
                DateOfBirth = DateTime.Parse("1999-04-14"),
                Gender = "Female",
                IsCensorship = false,
                IsActive = true,
                RoleId = 1,
                Devicetoken = null,
                ManagerId = null
            };
            var request = new DeclineRequestRegisterAccountCommand { UserId = userId };
            var cancellationToken = CancellationToken.None;
            _mockAccountRepository.Setup(x => x.GetById(userId)).ReturnsAsync(account);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Thành công");

            _mockAccountRepository.Verify(x => x.GetById(userId), Times.Once);
            _mockAccountRepository.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_WithNonExistingAccount_ShouldReturnErrorResponse()
        {
            // Arrange
            var userId = 2;
            var request = new DeclineRequestRegisterAccountCommand { UserId = userId };
            var cancellationToken = CancellationToken.None;
            _mockAccountRepository.Setup(x => x.GetById(2)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy tài khoản");
            _mockAccountRepository.Verify(x => x.GetById(userId), Times.Once);
            _mockAccountRepository.Verify(x => x.Save(), Times.Never);

        }
    }
}

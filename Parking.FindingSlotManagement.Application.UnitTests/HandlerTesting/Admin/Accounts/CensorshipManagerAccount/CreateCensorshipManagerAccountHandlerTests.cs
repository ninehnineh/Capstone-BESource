using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Accounts.CensorshipManagerAccount
{
    public class CreateCensorshipManagerAccountHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly CreateNewCensorshipManagerAccountCommandHandler _sut;

        public CreateCensorshipManagerAccountHandlerTests()
        {
            // Setup AutoMapper
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mapperConfiguration.CreateMapper();

            // Setup mock repository and service
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockEmailService = new Mock<IEmailService>();

            // Instantiate system under test
            var x = _mockAccountRepository.Object;
            _sut = new CreateNewCensorshipManagerAccountCommandHandler(_mockAccountRepository.Object, _mockEmailService.Object);
        }
        [Fact]
        public async Task Handle_WhenAccountExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewCensorshipManagerAccountCommand { Email = "linhdase151281@fpt.edu.vn"};
            var existingAccount = new User { Email = "linhdase151281@fpt.edu.vn"};
            _mockAccountRepository.Setup(x => x.GetItemWithCondition(x => x.Email.Equals(request.Email), null, true)).ReturnsAsync(existingAccount);

            // Act
            var response = await _sut.Handle(request, CancellationToken.None);

            // Assert
            response.StatusCode.ShouldBe(400);
            response.Success.ShouldBeFalse();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Email đã tồn tại. Vui lòng nhập lại email!!!");

            _mockAccountRepository.Verify(x => x.Insert(It.IsAny<User>()), Times.Never);
            _mockEmailService.Verify(x => x.SendMail(It.IsAny<EmailModel>()), Times.Never);
        }
        /*[Fact]
        public async Task Handle_WhenAccountDoesNotExist_CreatesNewAccountAndSendsEmail()
        {
            // Arrange
            var request = new CreateNewCensorshipManagerAccountCommand { Name="Example", Email = "newaccount@example.com", Password = "password123", Phone = "0111111111", Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTj99rBmV_qbeBomsSxk3d3Z6FQ3UrbvhKeew&usqp=CAU", DateOfBirth = DateTime.Parse("1995-05-04"), Gender = "Male" };
            var newUser = _mapper.Map<User>(request);
            _mockAccountRepository.Setup(x => x.GetItemWithCondition(x => x.Email.Equals(request.Email), null, true)).ReturnsAsync((User)null);

            // Act
            var response = await _sut.Handle(request, CancellationToken.None);

            // Assert
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            response.Data.ShouldBe(newUser.UserId);

            _mockAccountRepository.Verify(x => x.Insert(newUser), Times.Once);
            _mockEmailService.Verify(x => x.SendMail(It.Is<EmailModel>(e => e.To == newUser.Email && e.Subject == "Tài khoản đã được doanh nghiêp ParkZ thông qua." && e.Body.Contains("Email: newaccount@example.com") && e.Body.Contains("Password: password123"))), Times.Once);
        }*/
    }
}

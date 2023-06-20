//using FluentValidation.TestHelper;
//using Moq;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.CreateNewStaffParking;
//using Parking.FindingSlotManagement.Domain.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.StaffPakings.StaffParkingManagement
//{
//    public class CreateNewStaffParkingCommandHandlerTests
//    {
//        private readonly Mock<IStaffParkingRepository> _staffParkingRepositoryMock;
//        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
//        private readonly Mock<IAccountRepository> _accountRepositoryMock;
//        private readonly CreateNewStaffParkingCommandHandler _handler;
//        private readonly CreateNewStaffParkingCommandValidation _validator;
//        public CreateNewStaffParkingCommandHandlerTests()
//        {
//            _staffParkingRepositoryMock = new Mock<IStaffParkingRepository>();
//            _parkingRepositoryMock = new Mock<IParkingRepository>();
//            _accountRepositoryMock = new Mock<IAccountRepository>();
//            _validator = new CreateNewStaffParkingCommandValidation();
//            _handler = new CreateNewStaffParkingCommandHandler(_staffParkingRepositoryMock.Object, _accountRepositoryMock.Object, _parkingRepositoryMock.Object);
//        }
    
//        [Fact]
//        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
//        {
//            // Arrange
//            var request = new CreateNewStaffParkingCommand
//            {
//                UserId = 7,
//                ParkingId = 5
//            };
//            var expectedParking = new Domain.Entities.StaffParking
//            {
//                StaffParkingId = 2,
//                UserId = 7,
//                ParkingId = 5
//            };
//            var userExist = new User { UserId = 7 };
//            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(userExist);
//            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
//            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
//                .ReturnsAsync(parkingExist);


//            // Act
//            var response = await _handler.Handle(request, CancellationToken.None);

//            // Assert
//            response.ShouldNotBeNull();
//            response.StatusCode.ShouldBe(201);
//            response.Success.ShouldBeTrue();
//            response.Count.ShouldBe(0);
//            response.Message.ShouldBe("Thành công");
//            // Verify that the account repository was called to insert the new account
//            _staffParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.StaffParking>()), Times.Once);
//        }
//        [Fact]
//        public async Task Handle_InvalidUserId_ReturnsErrorResponse()
//        {
//            // Arrange
//            var command = new CreateNewStaffParkingCommand
//            {
//                UserId = 2,
//                ParkingId = 5
//            };
//            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync((User)null);
//            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
//            _parkingRepositoryMock.Setup(x => x.GetById(command.ParkingId))
//                .ReturnsAsync(parkingExist);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.Data.ShouldBe(0);
//            result.Message.ShouldBe("Không tìm thấy tài khoản.");
//            result.StatusCode.ShouldBe(200);

//            _staffParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.StaffParking>()), Times.Never);
//        }
//        [Fact]
//        public async Task Handle_InvalidParkingId_ReturnsErrorResponse()
//        {
//            // Arrange
//            var command = new CreateNewStaffParkingCommand
//            {
//                UserId = 7,
//                ParkingId = 1
//            };

//            var userExist = new User { UserId = 7 };
//            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync(userExist);
//            _parkingRepositoryMock.Setup(x => x.GetById(command.ParkingId))
//                .ReturnsAsync((Domain.Entities.Parking)null);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Success.ShouldBeTrue();
//            result.Data.ShouldBe(0);
//            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
//            result.StatusCode.ShouldBe(200);

//            _staffParkingRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.StaffParking>()), Times.Never);
//        }
//        [Fact]
//        public void UserId_ShouldNotBeEmpty()
//        {
//            var command = new CreateNewStaffParkingCommand
//            {
//                UserId = null,
//                ParkingId = 5
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.UserId);
//        }
//        [Fact]
//        public void UserId_ShouldNotBeNull()
//        {
//            var command = new CreateNewStaffParkingCommand
//            {
//                ParkingId = 5
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.UserId);
//        }
//        [Fact]
//        public void ParkingId_ShouldNotBeEmpty()
//        {
//            var command = new CreateNewStaffParkingCommand
//            {
//                UserId = 7,
//                ParkingId = null
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
//        }
//        [Fact]
//        public void ParkingId_ShouldNotBeNull()
//        {
//            var command = new CreateNewStaffParkingCommand
//            {
//                UserId = 7,
//            };

//            var result = _validator.TestValidate(command);

//            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
//        }
//    }
//}

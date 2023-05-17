using AutoMapper;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class CreateParkingPriceCommandHandlerTests
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateParkingPriceCommandValidation _validator;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        public CreateParkingPriceCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _mapperMock = new Mock<IMapper>();
            _validator = new CreateParkingPriceCommandValidation(_parkingPriceRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenValidData()
        {
            var command = new CreateParkingPriceCommand { BusinessId = 1, ParkingPriceName = "Test"};

            var entity = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = 1,
                ParkingPriceName = command.ParkingPriceName,
                IsActive = true,
                UserId = command.BusinessId,
            };

            _mapperMock.Setup(x => x.Map<Domain.Entities.ParkingPrice>(command)).Returns(entity);

            _parkingPriceRepositoryMock.Setup(x => x.Insert(entity));

            var service = new CreateParkingPriceCommandHandler(_parkingPriceRepositoryMock.Object, _mapperMock.Object);

            var result = await service.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Data.ShouldBe(entity.ParkingPriceId);
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(201);
            result.Success.ShouldBeTrue();

            _mapperMock.Verify(m => m.Map<Domain.Entities.ParkingPrice>(command), Times.Once);
            _parkingPriceRepositoryMock.Verify(r => r.Insert(entity), Times.Once);
        }

        [Fact]
        public async Task Should_Have_Error_When_ParkingPriceName_Is_Null()
        {
            // Arrange
            var command = new CreateParkingPriceCommand { BusinessId = 1, ParkingPriceName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ParkingPriceName)
                .WithErrorMessage("Vui lòng nhập Parking Price Name.")
                .WithSeverity(Severity.Error);
        }

    }
}

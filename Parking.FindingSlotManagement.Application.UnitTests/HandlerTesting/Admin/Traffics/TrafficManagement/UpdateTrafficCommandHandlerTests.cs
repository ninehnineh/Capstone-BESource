using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Traffics.TrafficManagement
{
    public class UpdateTrafficCommandHandlerTests
    {
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly UpdateTrafficCommandHandler _handler;
        private readonly UpdateTrafficCommandValidation _validator;

        public UpdateTrafficCommandHandlerTests()
        {
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new UpdateTrafficCommandHandler(_trafficRepositoryMock.Object);
            _validator = new UpdateTrafficCommandValidation();
        }
        [Fact]
        public async Task UpdateTrafficCommandHandler_Should_Update_Traffic_Successfully()
        {
            // Arrange
            var request = new UpdateTrafficCommand
            {
                TrafficId = 1,
                Name = "Xe container"
            };
            var cancellationToken = new CancellationToken();
            var Oldtraffic = new Traffic
            {
                TrafficId = 1,
                Name = "Xe ô tô",
                IsActive = true,
            };
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId))
                .ReturnsAsync(Oldtraffic);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            Oldtraffic.Name.ShouldBe(request.Name);
            _trafficRepositoryMock.Verify(x => x.Update(Oldtraffic), Times.Once);
        }
        [Fact]
        public async Task UpdateTrafficCommandHandler_Should_Return_Not_Found_If_Traffic_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateTrafficCommand
            {
                TrafficId = 2000
            };
            var cancellationToken = new CancellationToken();
            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId))
                .ReturnsAsync((Traffic)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _trafficRepositoryMock.Verify(x => x.Update(It.IsAny<Traffic>()), Times.Never);
        }
        [Fact]
        public async Task UpdateTrafficCommandHandler_Should_Return_Name_Already_Exists()
        {
            // Arrange
            var command = new UpdateTrafficCommand
            {
                TrafficId = 1,
                Name = "Xe máy", // This name already exists
                
            };
            _trafficRepositoryMock.Setup(x => x.GetById(command.TrafficId))
                .ReturnsAsync(new Traffic
                {
                    TrafficId = command.TrafficId,
                    Name = "Xe ô tô" // Existing email for the account
                });
            _trafficRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Traffic, bool>>>(), null, true))
                .ReturnsAsync(new Traffic
                {
                    TrafficId = 1,
                    Name = command.Name // Name already exists for another traffic
                });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Tên phương tiện đã tồn tại. Hãy nhập tên khác!!!");
            _trafficRepositoryMock.Verify(x => x.Update(It.IsAny<Traffic>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateTrafficCommand
            {
                TrafficId = 1,
                Name = "lorem loremloremlorem lorem lorem lorem lorem lorem lorem lorem loremv lorem lorem lorem",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}

using AutoMapper;
using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic;
using Parking.FindingSlotManagement.Application.Mapping;
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
    public class CreateNewTrafficCommandHandlerTests
    {
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly CreateNewTrafficCommandHandler _handler;
        private readonly CreateNewTrafficCommandValidation _validator;
        private readonly IMapper _mapper;
        MapperConfiguration configuration;

        public CreateNewTrafficCommandHandlerTests()
        {
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new CreateNewTrafficCommandHandler(_trafficRepositoryMock.Object);
            _validator = new CreateNewTrafficCommandValidation();
            configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = configuration.CreateMapper();
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewTrafficCommand { Name = "Xe container" };
            var expectedTraffic = new User { Name = "Xe container" };
            _trafficRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Traffic, bool>>>(), null, true))
                .ReturnsAsync((Traffic)null);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            // Verify that the account repository was called to insert the new account
            _trafficRepositoryMock.Verify(x => x.Insert(It.Is<Traffic>(traffic => traffic.Name == expectedTraffic.Name)));
        }
        [Fact]
        public async Task Handle_WhenTrafficExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateNewTrafficCommand { Name = "Xe ô tô" };
            var existingTraffic = new Traffic { Name = "Xe ô tô" };
            _trafficRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Traffic, bool>>>(), null, true))
                .ReturnsAsync(existingTraffic);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(400);
            response.Success.ShouldBeFalse();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Tên phương tiện đã tồn tại. Vui lòng nhập lại tên phương tiện!!!");
            
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewTrafficCommand
            {
                Name = "",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewTrafficCommand();

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewTrafficCommand
            {
                Name = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}

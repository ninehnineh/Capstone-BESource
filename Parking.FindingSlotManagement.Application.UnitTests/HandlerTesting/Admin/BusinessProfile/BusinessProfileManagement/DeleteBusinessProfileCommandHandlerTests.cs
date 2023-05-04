using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Commands.DeleteBusinessProfile;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.BusinessProfile.BusinessProfileManagement
{
    public class DeleteBusinessProfileCommandHandlerTests
    {
        private readonly Mock<IBusinessProfileRepository> _mockBusinessProfileRepository;
        private readonly DeleteBusinessProfileCommandHandler _handler;
        public DeleteBusinessProfileCommandHandlerTests()
        {
            _mockBusinessProfileRepository = new Mock<IBusinessProfileRepository>();
            _handler = new DeleteBusinessProfileCommandHandler(_mockBusinessProfileRepository.Object);
        }
        [Fact]
        public async Task Handle_DeleteBusinessProfileCommand_Should_Return_SuccessfulResponse()
        {
            //Arrange
            var businessProfile = new Domain.Entities.BusinessProfile()
            {
                BusinessProfileId = 1,
                Name = "Bãi đỗ xe Pasteur",
                Address = "Pasteur, Thành phố Hồ Chí Minh, Việt Nam",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 10
            };
            var command = new DeleteBusinessProfileCommand { BusinessProfileId = 1 };
            _mockBusinessProfileRepository.Setup(x => x.GetById(command.BusinessProfileId)).ReturnsAsync(businessProfile);
            _mockBusinessProfileRepository.Setup(x => x.Delete(businessProfile)).Returns(Task.CompletedTask);
            

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Message.ShouldBe("Thành công");
            _mockBusinessProfileRepository.Verify(x => x.GetById(businessProfile.BusinessProfileId), Times.Once);
            _mockBusinessProfileRepository.Verify(x => x.Delete(businessProfile), Times.Once);

        }
        [Fact]
        public async Task Handle_WithNonExistingAccount_ShouldReturnErrorResponse()
        {
            // Arrange
            var businessProfileId = 10000;
            var request = new DeleteBusinessProfileCommand { BusinessProfileId = businessProfileId };
            var cancellationToken = CancellationToken.None;
            _mockBusinessProfileRepository.Setup(x => x.GetById(businessProfileId)).ReturnsAsync((Domain.Entities.BusinessProfile)null);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy thông tin doanh nghiệp");
            _mockBusinessProfileRepository.Verify(x => x.GetById(businessProfileId), Times.Once);

        }
    }
}

using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.BusinessProfile.GetBusinessProfileById
{
    public class GetBusinessProfileByIdQueryHandlerTests
    {
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly GetBusinessProfileByIdQueryHandler _handler;
        public GetBusinessProfileByIdQueryHandlerTests()
        {
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _handler = new GetBusinessProfileByIdQueryHandler(_businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetBusinessProfileByIdQuery()
            {
                BusinessProfileId = 1
            };
            var businessProfiles = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId = 1,
                Name = "Bãi đỗ xe Pasteur",
                Address = "Pasteur, Thành phố Hồ Chí Minh, Việt Nam",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 10
            };
            _businessProfileRepositoryMock.Setup(x => x.GetById(query.BusinessProfileId)).ReturnsAsync(businessProfiles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidId_ReturnsNotFound()
        {
            var query = new GetBusinessProfileByIdQuery()
            {
                BusinessProfileId = 9999999
            };
            _businessProfileRepositoryMock.Setup(x => x.GetById(query.BusinessProfileId)).ReturnsAsync((Domain.Entities.BusinessProfile)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.ShouldBeNull<GetBusinessProfileByIdResponse>();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy thông tin doanh nghiệp");
        }
    }
}

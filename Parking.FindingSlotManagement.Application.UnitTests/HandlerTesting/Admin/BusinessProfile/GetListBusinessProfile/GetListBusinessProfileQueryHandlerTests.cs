using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.BusinessProfile.GetListBusinessProfile
{
    public class GetListBusinessProfileQueryHandlerTests
    {
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly GetListBusinessProfileQueryHandler _handler;
        public GetListBusinessProfileQueryHandlerTests()
        {
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _handler = new GetListBusinessProfileQueryHandler(_businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListBusinessProfileQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var businessProfiles = new List<Domain.Entities.BusinessProfile>
            {
                new Domain.Entities.BusinessProfile
                {
                    BusinessProfileId = 1,
                    Name = "Bãi đỗ xe Pasteur",
                    Address = "Pasteur, Thành phố Hồ Chí Minh, Việt Nam",
                    FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                    BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                    BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                    UserId = 10
                }
            };
            _businessProfileRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.BusinessProfile, object>>>>(), x => x.BusinessProfileId, true, request.PageNo, request.PageSize)).ReturnsAsync(businessProfiles);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListBusinessProfileQuery
            {
                PageNo = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task Handle_WithNoBusinessProfileFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListBusinessProfileQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var businessProfiles = new List<Domain.Entities.BusinessProfile>();
            _businessProfileRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.BusinessProfile, object>>>>(), x => x.BusinessProfileId, true, request.PageNo, request.PageSize)).ReturnsAsync(businessProfiles);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}

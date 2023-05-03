using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileByUserId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.BusinessProfile.BusinessProfileManagement
{
    public class GetBusinessProfileByUserIdQueryHandlerTests
    {
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly GetBusinessProfileByUserIdQueryHandler _handler;
        public GetBusinessProfileByUserIdQueryHandlerTests()
        {
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new GetBusinessProfileByUserIdQueryHandler(_businessProfileRepositoryMock.Object, _accountRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetBusinessProfileByUserIdQuery()
            {
                UserId = 7
            };
            var businessProfile = new Domain.Entities.BusinessProfile
            {
                BusinessProfileId = 2,
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };
            var UserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(query.UserId)).ReturnsAsync(UserExist);
            _businessProfileRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true)).ReturnsAsync(businessProfile);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidUserId_ReturnsNotFound()
        {
            var query = new GetBusinessProfileByUserIdQuery()
            {
                UserId = 999999999
            };
            _accountRepositoryMock.Setup(x => x.GetById(query.UserId)).ReturnsAsync((User)null);
            _businessProfileRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true)).ReturnsAsync((Domain.Entities.BusinessProfile)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
        }
    }
}

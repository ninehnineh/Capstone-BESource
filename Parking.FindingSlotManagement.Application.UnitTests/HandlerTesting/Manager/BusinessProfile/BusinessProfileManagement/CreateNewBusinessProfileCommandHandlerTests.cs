using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Commands.CreateNewBusinessProfile;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.BusinessProfile.BusinessProfileManagement
{
    public class CreateNewBusinessProfileCommandHandlerTests
    {
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly CreateNewBusinessProfileCommandHandler _handler;
        private readonly CreateNewBusinessProfileCommandValidation _validator;
        public CreateNewBusinessProfileCommandHandlerTests()
        {
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _validator = new CreateNewBusinessProfileCommandValidation();
            _handler = new CreateNewBusinessProfileCommandHandler(_businessProfileRepositoryMock.Object, _accountRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var checkUserExist = new User { UserId = 7 };
            _accountRepositoryMock.Setup(x => x.GetById(request.UserId)).ReturnsAsync(checkUserExist);



            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _businessProfileRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.BusinessProfile>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidUserId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 2
            };

            _accountRepositoryMock.Setup(x => x.GetById(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.StatusCode.ShouldBe(200);

            _businessProfileRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.BusinessProfile>()), Times.Never);
        }
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh PhúcBãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Fact]
        public void Address_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void Address_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Fact]
        public void FrontIdentification_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FrontIdentification);
        }
        [Fact]
        public void FrontIdentification_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FrontIdentification);
        }
        [Fact]
        public void BackIdentification_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BackIdentification);
        }
        [Fact]
        public void BackIdentification_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BackIdentification);
        }
        [Fact]
        public void BusinessLicense_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessLicense);
        }
        [Fact]
        public void BusinessLicense_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                UserId = 7
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.BusinessLicense);
        }
        [Fact]
        public void UserId_ShouldNotBeEmpty()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
                UserId = null,
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
        [Fact]
        public void UserId_ShouldNotBeNull()
        {
            var command = new CreateNewBusinessProfileCommand
            {
                Name = "Bãi Giữ Xe Minh Phúc",
                Address = "48 Đường 475, Phước Long B, Quận 9, Thành phố Hồ Chí Minh",
                FrontIdentification = "https://vemaybaybackinh.net/assets/uploads/2019/01/th%E1%BA%BB-c%C4%83n-c%C6%B0%E1%BB%9Bc.jpg",
                BackIdentification = "https://static.cand.com.vn/Files/Image/phulu/2021/03/11/thumb_660_d2cb0712-aea1-4b37-bd7a-76a27ef461f0.jpg",
                BusinessLicense = "https://thanhlapdoanhnghiepvn.vn/Uploads/images/dang-ky-giay-phep-kinh-doanh-doanh-nghiep(1).jpg",
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}

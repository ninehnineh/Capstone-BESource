using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.RatingParking.RatingManagement.Commands.UpdateRatingStarsOfParking
{
    public class UpdateRatingStarsOfParkingCommandHandler : IRequestHandler<UpdateRatingStarsOfParkingCommand, ServiceResponse<string>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IBookingRepository _bookingRepository;

        public UpdateRatingStarsOfParkingCommandHandler(IParkingRepository parkingRepository, IBookingRepository bookingRepository)
        {
            _parkingRepository = parkingRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<ServiceResponse<string>> Handle(UpdateRatingStarsOfParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkBookingExist = await _bookingRepository.GetById(request.BookingId);
                if(checkBookingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy đơn đặt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkParkingExist.StarsCount == null)
                {
                    checkParkingExist.StarsCount = 0;
                }
                if (checkParkingExist.Stars == null)
                {
                    checkParkingExist.Stars = 0;
                }
                if (checkParkingExist.TotalStars == null)
                {
                    checkParkingExist.TotalStars = 0;
                }
                checkParkingExist.StarsCount += 1;
                checkParkingExist.TotalStars += request.Stars ;
                // Hàm làm tròn chữ số thập phân thứ nhất
                var ope = checkParkingExist.TotalStars / checkParkingExist.StarsCount;
                checkParkingExist.Stars = (float)Math.Round((float)ope, 1, MidpointRounding.AwayFromZero);
                await _parkingRepository.Save();
                checkBookingExist.IsRating = true;
                await _bookingRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

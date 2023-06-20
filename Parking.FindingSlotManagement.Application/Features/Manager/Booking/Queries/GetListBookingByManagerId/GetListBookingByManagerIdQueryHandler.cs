//using AutoMapper;
//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Mapping;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId
//{
//    public class GetListBookingByManagerIdQueryHandler : IRequestHandler<GetListBookingByManagerIdQuery, ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>>
//    {
//        private readonly IBookingRepository _bookingRepository;
//        MapperConfiguration config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new MappingProfile());
//        });
//        public GetListBookingByManagerIdQueryHandler(IBookingRepository bookingRepository)
//        {
//            _bookingRepository = bookingRepository;
//        }
//        public async Task<ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>> Handle(GetListBookingByManagerIdQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                if (request.PageNo <= 0)
//                {
//                    request.PageNo = 1;
//                }
//                if (request.PageSize <= 0)
//                {
//                    request.PageSize = 1;
//                }
//                List<Expression<Func<Domain.Entities.Booking, object>>> includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
//                {
//                    x => x.User,
//                    x => x.ParkingSlot,
//                    x => x.VehicleInfor,
//                    x => x.ParkingSlot.Floor.Parking
//                };
//                var lstBooking = await _bookingRepository.GetAllItemWithPagination(x => x.ParkingSlot.Floor.Parking.ManagerId == request.ManagerId, includes, x => x.BookingId, true, request.PageNo, request.PageSize);
//                if(lstBooking.Count() <= 0)
//                {
//                    return new ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>
//                    {
//                        Message = "Không tìm thấy.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                }
//                var _mapper = config.CreateMapper();
//                var lstDto = _mapper.Map<IEnumerable<GetListBookingByManagerIdResponse>>(lstBooking);
//                return new ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>
//                {
//                    Data = lstDto,
//                    Message = "Thành công",
//                    StatusCode = 200,
//                    Success = true,
//                    Count = lstDto.Count()
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}

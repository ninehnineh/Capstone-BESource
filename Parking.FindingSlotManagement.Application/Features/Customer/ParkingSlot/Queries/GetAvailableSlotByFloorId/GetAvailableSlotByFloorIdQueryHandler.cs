using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId
{
    public class GetAvailableSlotByFloorIdQueryHandler : IRequestHandler<GetAvailableSlotByFloorIdQuery, ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetAvailableSlotByFloorIdQueryHandler(IFloorRepository floorRepository, IParkingSlotRepository parkingSlotRepository, ITimeSlotRepository timeSlotRepository)
        {
            _floorRepository = floorRepository;
            _parkingSlotRepository = parkingSlotRepository;
            _timeSlotRepository = timeSlotRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>> Handle(GetAvailableSlotByFloorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _mapper = config.CreateMapper();
                var floorExist = await _floorRepository.GetById(request.FloorId);
                if (floorExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>
                    {
                        Message = "Không tìm thấy tầng.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                //get slot đã book 
                var includes = new List<Expression<Func<Domain.Entities.TimeSlot, object>>>
                {
                   x => x.Parkingslot,
                   x => x.Parkingslot.Floor
                };
                var currentLstBookedSlot = await _timeSlotRepository.GetAllItemWithCondition(x => 
                                                            x.Parkingslot.FloorId == request.FloorId && 
                                                            x.StartTime >= request.StartTimeBooking && 
                                                            x.EndTime <= request.EndTimeBooking && x.Status == "Booked", includes);
                HashSet<int> listParkingSlotIdExist = new();
                foreach (var item in currentLstBookedSlot)
                {
                    /*var bookedStartTime = item.StartTime.Hour;
                    var bookedEndTime = item.EndTime.Hour;
                    if (request.StartTimeBooking < item.EndTime && request.EndTimeBooking > item.StartTime)
                    {
                        listParkingSlotIdExist.Add((int)item.ParkingSlotId);
                    }*/
                    if (!listParkingSlotIdExist.Contains((int)item.ParkingSlotId))
                    {
                        listParkingSlotIdExist.Add((int)item.ParkingSlotId);
                    }
                }
                var lstParkingSlotHasBooked = await _parkingSlotRepository.GetAllItemWithCondition(x => listParkingSlotIdExist.Contains((int)x.ParkingSlotId) || x.FloorId == request.FloorId && x.IsAvailable == false);
                List<GetAvailableSlotByFloorIdResponse> lstTong = new List<GetAvailableSlotByFloorIdResponse>();
                foreach (var item in lstParkingSlotHasBooked)
                {
                    var entity = _mapper.Map<ParkingSlotDto>(item);
                    var BigEntity = new GetAvailableSlotByFloorIdResponse
                    {
                        ParkingSlotDto = entity,
                        IsBooked = true
                    };
                    lstTong.Add(BigEntity);
                }
                //slot chua book
                var lstParkingSlot = await _parkingSlotRepository
                    .GetAllItemWithCondition(x => x.FloorId == request.FloorId);
                var filterParkingSlot = lstParkingSlot
                    .Where(x => !listParkingSlotIdExist.Contains(x.ParkingSlotId)).ToList();
                var filter2ParkingSlot = filterParkingSlot
                    .Where(x => x.IsAvailable == true).ToList();
                foreach (var item in filter2ParkingSlot)
                {
                    var entity = _mapper.Map<ParkingSlotDto>(item);
                    var BigEntity = new GetAvailableSlotByFloorIdResponse
                    {
                        ParkingSlotDto = entity,
                        IsBooked = false
                    };
                    lstTong.Add(BigEntity);
                }
                return new ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>
                {
                    Data = lstTong,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                    Count = lstTong.Count(),
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

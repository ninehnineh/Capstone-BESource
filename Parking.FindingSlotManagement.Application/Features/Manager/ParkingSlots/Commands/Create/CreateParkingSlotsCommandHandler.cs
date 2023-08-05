using AutoMapper;
using Hangfire;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;

public class CreateParkingSlotsCommandHandler : IRequestHandler<CreateParkingSlotsCommand, ServiceResponse<int>>
{
    private readonly IMapper _mapper;
    private readonly IParkingSlotRepository _parkingSlotRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;

    public CreateParkingSlotsCommandHandler(IMapper mapper, IParkingSlotRepository parkingSlotRepository,
     ITimeSlotRepository timeSlotRepository)
    {
        _mapper = mapper;
        _parkingSlotRepository = parkingSlotRepository;
        _timeSlotRepository = timeSlotRepository;
    }

    public async Task<ServiceResponse<int>> Handle(CreateParkingSlotsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var checkSlotNameExist = await _parkingSlotRepository.isExists
            (
                new Models.ParkingSlot.ParkingSlotDTO
                {
                    Name = request.Name,
                    FloorId = request.FloorId,
                }
            );

            if (checkSlotNameExist)
            {
                return new ServiceResponse<int>
                {
                    Message = "Tên chỗ để xe đã tồn tại",
                    StatusCode = 400,
                };
            }

            var a = _mapper.Map<ParkingSlot>(request);
            await _parkingSlotRepository.Insert(a);
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.AddDays(7);
            List<TimeSlot> ts = new List<TimeSlot>();
            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                for (int i = 0; i < 24; i++)
                {
                    DateTime startTime = date.Date + TimeSpan.FromHours(i);
                    DateTime endTime = date.Date + TimeSpan.FromHours(i + 1);

                    var entityTimeSlot = new TimeSlot
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        CreatedDate = DateTime.UtcNow.Date,
                        Status = "Free",
                        ParkingSlotId = a.ParkingSlotId
                    };
                    ts.Add(entityTimeSlot);
                }
            }
            var res = await _timeSlotRepository.AddRangeTimeSlot(ts);
            if (!res.Equals("Thành công"))
            {
                return new ServiceResponse<int>
                {
                    Message = "Quá trình thêm mới xảy ra lỗi!!!",
                    Success = false,
                    StatusCode = 400
                };
            }

            var timeToDelete = DateTime.UtcNow.AddHours(7).AddDays(7).Date - DateTime.UtcNow.AddHours(7);

            var deleteJobId = BackgroundJob.Schedule<IServiceManagement>(x => x.UpdateTimeSlotIn1Week(a.ParkingSlotId), timeToDelete);
            /*BackgroundJob.ContinueJobWith<IServiceManagement>(deleteJobId, x => x.AddTimeSlotInFuture(a.ParkingSlotId));*/

            Console.WriteLine($"One week ago to update time slot: {timeToDelete}");

            return new ServiceResponse<int>
            {
                Data = a.ParkingSlotId,
                Message = "Thành công",
                StatusCode = 201,
                Success = true,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
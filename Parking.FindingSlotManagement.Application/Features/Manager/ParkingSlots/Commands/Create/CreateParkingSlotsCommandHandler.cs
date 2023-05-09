using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;

public class CreateParkingSlotsCommandHandler : IRequestHandler<CreateParkingSlotsCommand, ServiceResponse<int>>
{
    private readonly IMapper _mapper;
    private readonly IParkingSlotRepository _parkingSlotRepository;

    public CreateParkingSlotsCommandHandler(IMapper mapper, IParkingSlotRepository parkingSlotRepository)
    {
        _mapper = mapper;
        _parkingSlotRepository = parkingSlotRepository;
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
                    ParkingId = request.ParkingId,
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
            return new ServiceResponse<int>
            {
                Data = a.ParkingSlotId,
                Message = "Thành công",
                StatusCode = 201,
                Success = true,
                Count = 0
            };

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
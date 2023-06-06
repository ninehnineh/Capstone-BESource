using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice
{
    public class CreateParkingPriceCommandHandler : IRequestHandler<CreateParkingPriceCommand, ServiceResponse<int>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateParkingPriceCommandHandler(IParkingPriceRepository parkingPriceRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
        }

        public async Task<ServiceResponse<int>> Handle(CreateParkingPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<Domain.Entities.ParkingPrice>(request);

                entity.IsActive = true;

                await _parkingPriceRepository.Insert(entity);

                return new ServiceResponse<int>
                {
                    Data = entity.ParkingPriceId,
                    Message = "Thành công",
                    StatusCode = 201,
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

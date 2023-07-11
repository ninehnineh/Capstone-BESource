using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.CreateNewFee
{
    public class CreateNewFeeCommandHandler : IRequestHandler<CreateNewFeeCommand, ServiceResponse<int>>
    {
        private readonly IFeeRepository _feeRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public CreateNewFeeCommandHandler(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<Domain.Entities.Fee>(request);
                await _feeRepository.Insert(entity);
                return new ServiceResponse<int>
                {
                    Message = "Thành công",
                    StatusCode = 201,
                    Success = true,
                    Data = entity.FeeId
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

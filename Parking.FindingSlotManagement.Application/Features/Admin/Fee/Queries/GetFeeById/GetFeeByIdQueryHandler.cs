using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetFeeById
{
    public class GetFeeByIdQueryHandler : IRequestHandler<GetFeeByIdQuery, ServiceResponse<GetFeeByIdResponse>>
    {
        private readonly IFeeRepository _feeRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetFeeByIdQueryHandler(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<GetFeeByIdResponse>> Handle(GetFeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var feeExist = await _feeRepository.GetById(request.FeeId);
                if (feeExist == null)
                {
                    return new ServiceResponse<GetFeeByIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<GetFeeByIdResponse>(feeExist);
                return new ServiceResponse<GetFeeByIdResponse>
                {
                    Data = entity,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

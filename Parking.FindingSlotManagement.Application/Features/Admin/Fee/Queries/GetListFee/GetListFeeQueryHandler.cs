using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetListFee
{
    public class GetListFeeQueryHandler : IRequestHandler<GetListFeeQuery, ServiceResponse<IEnumerable<GetListFeeResponse>>>
    {
        private readonly IFeeRepository _feeRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListFeeQueryHandler(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListFeeResponse>>> Handle(GetListFeeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lst = await _feeRepository.GetAllItemWithCondition(null, null, x => x.FeeId, true);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListFeeResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }

                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<IEnumerable<GetListFeeResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetListFeeResponse>>
                {
                    Data = entity,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = entity.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

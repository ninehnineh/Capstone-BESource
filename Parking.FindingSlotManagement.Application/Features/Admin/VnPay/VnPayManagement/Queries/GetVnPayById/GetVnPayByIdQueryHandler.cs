using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayById
{
    public class GetVnPayByIdQueryHandler : IRequestHandler<GetVnPayByIdQuery, ServiceResponse<GetVnPayByIdResponse>>
    {
        private readonly IVnPayRepository _vnPayRepository;
        private readonly IMapper _mapper;

        public GetVnPayByIdQueryHandler(IVnPayRepository vnPayRepository, IMapper mapper)
        {
            _vnPayRepository = vnPayRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetVnPayByIdResponse>> Handle(GetVnPayByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vnPayExist = await _vnPayRepository.GetById(request.VnPayId);
                if(vnPayExist == null)
                {
                    return new ServiceResponse<GetVnPayByIdResponse>
                    {
                        Message = "Không tìm thấy thông tin.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var resReturn = _mapper.Map<GetVnPayByIdResponse>(vnPayExist);
                return new ServiceResponse<GetVnPayByIdResponse>
                {
                    Data = resReturn,
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

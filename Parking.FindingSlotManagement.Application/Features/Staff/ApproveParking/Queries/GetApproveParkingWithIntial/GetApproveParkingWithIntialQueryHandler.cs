using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingWithIntial
{
    public class GetApproveParkingWithIntialQueryHandler : IRequestHandler<GetApproveParkingWithIntialQuery, ServiceResponse<GetApproveParkingWithIntialResponse>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;

        public GetApproveParkingWithIntialQueryHandler(IParkingRepository parkingRepository, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetApproveParkingWithIntialResponse>> Handle(GetApproveParkingWithIntialQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.Parking, object>>> includes = new()
                {
                    x => x.ApproveParkings
                };
                var parkingExist = await _parkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId, includes);
                if(parkingExist == null)
                {
                    return new ServiceResponse<GetApproveParkingWithIntialResponse>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var entity = parkingExist.ApproveParkings.LastOrDefault(x => x.Status.Equals(ApproveParkingStatus.Tạo_mới.ToString()));
                if(entity == null)
                {
                    return new ServiceResponse<GetApproveParkingWithIntialResponse>
                    {
                        Message = "Không tìm thấy đơn tạo mới.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var entityRes = _mapper.Map<GetApproveParkingWithIntialResponse>(entity);
                return new ServiceResponse<GetApproveParkingWithIntialResponse>
                {
                    Data = entityRes,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

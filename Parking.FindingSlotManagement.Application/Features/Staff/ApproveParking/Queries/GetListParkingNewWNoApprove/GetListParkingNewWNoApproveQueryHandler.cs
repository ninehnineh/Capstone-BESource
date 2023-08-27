using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListParkingNewWNoApprove
{
    public class GetListParkingNewWNoApproveQueryHandler : IRequestHandler<GetListParkingNewWNoApproveQuery, ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;

        public GetListParkingNewWNoApproveQueryHandler(IParkingRepository parkingRepository, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>> Handle(GetListParkingNewWNoApproveQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                List<Expression<Func<Domain.Entities.Parking, object>>> includes = new()
                {
                    x => x.ApproveParkings
                };
                var lst = await _parkingRepository.GetAllItemWithPagination(x => x.ApproveParkings.Count() == 0 && x.IsActive == false
                || x.ApproveParkings.OrderByDescending(x => x.ApproveParkingId).FirstOrDefault().Status.Equals(ApproveParkingStatus.Tạo_mới.ToString()) || x.ApproveParkings.OrderByDescending(x => x.ApproveParkingId).FirstOrDefault().Status.Equals(ApproveParkingStatus.Từ_chối.ToString()) && x.IsActive == false, includes, x => x.ParkingId, true, request.PageNo, request.PageSize);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var resReturn = _mapper.Map<IEnumerable<GetListParkingNewWNoApproveResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>
                {
                    Data = resReturn,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = resReturn.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

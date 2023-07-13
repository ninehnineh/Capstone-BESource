using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetListParkingWaitingToAccept
{
    public class GetListParkingWaitingToAcceptQueryHandler : IRequestHandler<GetListParkingWaitingToAcceptQuery, ServiceResponse<IEnumerable<GetListParkingWaitingToAcceptResponse>>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IMapper _mapper;

        public GetListParkingWaitingToAcceptQueryHandler(IApproveParkingRepository approveParkingRepository, IMapper mapper)
        {
            _approveParkingRepository = approveParkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetListParkingWaitingToAcceptResponse>>> Handle(GetListParkingWaitingToAcceptQuery request, CancellationToken cancellationToken)
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
                List<Expression<Func<Domain.Entities.ApproveParking, object>>> includes = new()
                {
                    x => x.User,
                    x => x.Parking
                };
                var lst = await _approveParkingRepository.GetAllItemWithPagination(x => x.Status.Equals(Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString()), includes, x => x.ApproveParkingId, true, request.PageNo, request.PageSize);
                if (!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListParkingWaitingToAcceptResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var entityReturn = _mapper.Map<IEnumerable<GetListParkingWaitingToAcceptResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetListParkingWaitingToAcceptResponse>>
                {
                    Data = entityReturn,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                    Count = entityReturn.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

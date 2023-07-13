using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById
{
    public class GetApproveParkingByIdQueryHandler : IRequestHandler<GetApproveParkingByIdQuery, ServiceResponse<GetApproveParkingByIdResponse>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IMapper _mapper;

        public GetApproveParkingByIdQueryHandler(IApproveParkingRepository approveParkingRepository, IMapper mapper)
        {
            _approveParkingRepository = approveParkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetApproveParkingByIdResponse>> Handle(GetApproveParkingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.ApproveParking, object>>> includes = new()
                {
                    x => x.User,
                    x => x.FieldWorkParkingImgs
                };
                var resp = await _approveParkingRepository.GetItemWithCondition(x => x.ApproveParkingId == request.ApproveParkingId, includes);
                if (resp == null)
                {
                    return new ServiceResponse<GetApproveParkingByIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                GetApproveParkingByIdResponse resReturn = new()
                {
                    StaffId = resp.StaffId,
                    StaffName = resp.User.Name,
                    ApproveParkingId = resp.ApproveParkingId,
                    Note = resp.Note,
                    CreatedDate = resp.CreatedDate,
                    Status = resp.Status,
                    Images = _mapper.Map<List<ImagesOfRequestApprove>>(resp.FieldWorkParkingImgs)
                };
                return new ServiceResponse<GetApproveParkingByIdResponse>
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

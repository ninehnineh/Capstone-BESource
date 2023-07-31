using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId
{
    public class GetFieldInforByParkingIdQueryHandler : IRequestHandler<GetFieldInforByParkingIdQuery, ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;

        public GetFieldInforByParkingIdQueryHandler(IParkingRepository parkingRepository, IMapper mapper, IApproveParkingRepository approveParkingRepository, IFieldWorkParkingImgRepository fieldWorkParkingImgRepository)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _approveParkingRepository = approveParkingRepository;
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>> Handle(GetFieldInforByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                List<Expression<Func<Domain.Entities.ApproveParking, object>>> includes = new List<Expression<Func<Domain.Entities.ApproveParking, object>>>
                {
                    x => x.User
                };
                var approveParkingInfo = await _approveParkingRepository.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, includes);
                if(!approveParkingInfo.Any())
                {
                    return new ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>
                    {
                        Message = "Chưa có thông tin thực địa.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                List<GetFieldInforByParkingIdResponse> resReturn = new();
                foreach (var item in approveParkingInfo)
                {
                    GetFieldInforByParkingIdResponse response = new()
                    {
                        ApproveParkingId = item.ApproveParkingId,
                        Note = item.Note,
                        StaffId = item.StaffId,
                        StaffName = item.User.Name,
                        Status = item.Status
                    };
                    resReturn.Add(response);
                }
                
                return new ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Data = resReturn
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

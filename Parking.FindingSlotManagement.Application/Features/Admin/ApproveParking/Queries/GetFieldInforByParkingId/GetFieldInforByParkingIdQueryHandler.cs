using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId
{
    public class GetFieldInforByParkingIdQueryHandler : IRequestHandler<GetFieldInforByParkingIdQuery, ServiceResponse<GetFieldInforByParkingIdResponse>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;

        public GetFieldInforByParkingIdQueryHandler(IParkingRepository parkingRepository, IApproveParkingRepository approveParkingRepository, IFieldWorkParkingImgRepository fieldWorkParkingImgRepository)
        {
            _parkingRepository = parkingRepository;
            _approveParkingRepository = approveParkingRepository;
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
        }
        public async Task<ServiceResponse<GetFieldInforByParkingIdResponse>> Handle(GetFieldInforByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<GetFieldInforByParkingIdResponse>
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
                var approveParkingInfo = await _approveParkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId, includes);
                List<string> imgs = new();
                var lstImgFromParking = await _fieldWorkParkingImgRepository.GetAllItemWithConditionByNoInclude(x => x.ApproveParkingId == approveParkingInfo.ApproveParkingId);
                if(!lstImgFromParking.Any())
                {
                    return new ServiceResponse<GetFieldInforByParkingIdResponse>
                    {
                        Message = "Chưa có hình ảnh thực địa.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                foreach (var item in lstImgFromParking)
                {
                    imgs.Add(item.Url);
                }
                GetFieldInforByParkingIdResponse response = new()
                {
                    ApproveParkingId = approveParkingInfo.ApproveParkingId,
                    Note = approveParkingInfo.Note,
                    StaffId = approveParkingInfo.StaffId,
                    StaffName = approveParkingInfo.User.Name,
                    Images = imgs
                };
                return new ServiceResponse<GetFieldInforByParkingIdResponse>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Data = response
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

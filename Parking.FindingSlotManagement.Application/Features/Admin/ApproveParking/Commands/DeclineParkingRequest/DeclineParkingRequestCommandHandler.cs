using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.DeclineParkingRequest
{
    public class DeclineParkingRequestCommandHandler : IRequestHandler<DeclineParkingRequestCommand, ServiceResponse<string>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        private readonly IParkingRepository _parkingRepository;

        public DeclineParkingRequestCommandHandler(IApproveParkingRepository approveParkingRepository, IParkingRepository parkingRepository)
        {
            _approveParkingRepository = approveParkingRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeclineParkingRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var approveParking = await _approveParkingRepository.GetItemWithCondition(x => x.ApproveParkingId == request.ApproveParkingId, null, false);
                if (approveParking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy yêu cầu xác thực của bãi.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (!approveParking.Status.Equals(Domain.Enum.ApproveParkingStatus.Chờ_duyệt.ToString()))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi đã được xử lý duyệt/từ chối, không thể thực hiện thao tác.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var parkingExist = await _parkingRepository.GetById(approveParking.ParkingId);
                parkingExist.IsActive = false;
                await _parkingRepository.Save();
                approveParking.Status = Domain.Enum.ApproveParkingStatus.Từ_chối.ToString();
                await _approveParkingRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.UpdateParkingSlots
{
    public class UpdateParkingSlotsCommandHandler : IRequestHandler<UpdateParkingSlotsCommand, ServiceResponse<string>>
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;

        public UpdateParkingSlotsCommandHandler(IParkingSlotRepository parkingSlotRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateParkingSlotsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _parkingSlotRepository.GetById(request.ParkingSlotId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy slot.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(!string.IsNullOrEmpty(request.RowIndex.ToString()))
                {
                    checkExist.RowIndex = request.RowIndex;
                }
                if(!string.IsNullOrEmpty(request.ColumnIndex.ToString()))
                {
                    checkExist.ColumnIndex = request.ColumnIndex;
                }
                await _parkingSlotRepository.Update(checkExist);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

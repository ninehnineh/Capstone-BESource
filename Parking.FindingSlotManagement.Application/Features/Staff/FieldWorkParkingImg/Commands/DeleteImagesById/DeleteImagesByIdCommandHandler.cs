using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.DeleteImagesById
{
    public class DeleteImagesByIdCommandHandler : IRequestHandler<DeleteImagesByIdCommand, ServiceResponse<string>>
    {
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;

        public DeleteImagesByIdCommandHandler(IFieldWorkParkingImgRepository fieldWorkParkingImgRepository)
        {
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteImagesByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _fieldWorkParkingImgRepository.GetById(request.FieldWorkParkingImgId);
                if(checkExist == null) 
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                await _fieldWorkParkingImgRepository.Delete(checkExist);
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

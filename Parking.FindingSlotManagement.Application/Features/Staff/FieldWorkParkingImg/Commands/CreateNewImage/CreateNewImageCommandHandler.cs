using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.CreateNewImage
{
    public class CreateNewImageCommandHandler : IRequestHandler<CreateNewImageCommand, ServiceResponse<int>>
    {
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;
        private readonly IApproveParkingRepository _approveParkingRepository;

        public CreateNewImageCommandHandler(IFieldWorkParkingImgRepository fieldWorkParkingImgRepository, IApproveParkingRepository approveParkingRepository)
        {
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
            _approveParkingRepository = approveParkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkApproveExist = await _approveParkingRepository.GetById(request.ApproveParkingId);
                if(checkApproveExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy yêu cầu.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (request.Images.Any())
                {
                    List<Domain.Entities.FieldWorkParkingImg> lstFWPI = new();
                    foreach (var item in request.Images)
                    {
                        Domain.Entities.FieldWorkParkingImg fwpi = new Domain.Entities.FieldWorkParkingImg
                        {
                            Url = item,
                            ApproveParkingId = request.ApproveParkingId
                        };
                        lstFWPI.Add(fwpi);
                    }
                    await _fieldWorkParkingImgRepository.AddRangeFieldWorkParkingImg(lstFWPI);
                }
                
                return new ServiceResponse<int>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

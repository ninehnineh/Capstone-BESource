using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Commands.DeleteBusinessProfile
{
    public class DeleteBusinessProfileCommandHandler : IRequestHandler<DeleteBusinessProfileCommand, ServiceResponse<string>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;

        public DeleteBusinessProfileCommandHandler(IBusinessProfileRepository businessProfileRepository)
        {
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteBusinessProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _businessProfileRepository.GetById(request.BusinessProfileId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp",
                        StatusCode = 200,
                        Success = true
                    };
                }
                await _businessProfileRepository.Delete(checkExist);
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

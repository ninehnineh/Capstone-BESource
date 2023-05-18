using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.DisableOrEnableTimeline
{
    public class DisableOrEnableTimelineCommandHandler : IRequestHandler<DisableOrEnableTimelineCommand, ServiceResponse<string>>
    {
        private readonly ITimelineRepository _timelineRepository;

        public DisableOrEnableTimelineCommandHandler(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableTimelineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkTimelineExist = await _timelineRepository.GetById(request.TimeLineId);
                if(checkTimelineExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy khung giờ.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkTimelineExist.IsActive == true)
                {
                    checkTimelineExist.IsActive = false;
                }
                else if(checkTimelineExist.IsActive == false)
                {
                    checkTimelineExist.IsActive = true;
                }
                await _timelineRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId
{
    public class GetListApproveParkingByParkingIdQueryHandler : IRequestHandler<GetListApproveParkingByParkingIdQuery, ServiceResponse<IEnumerable<GetListApproveParkingByParkingIdRes>>>
    {
        private readonly IApproveParkingRepository _approveParkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListApproveParkingByParkingIdQueryHandler(IApproveParkingRepository approveParkingRepository)
        {
            _approveParkingRepository = approveParkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListApproveParkingByParkingIdRes>>> Handle(GetListApproveParkingByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.ApproveParking, object>>> includes = new()
                {
                    x => x.User
                };
                var lst = await _approveParkingRepository.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, includes, x => x.ApproveParkingId, true);
                if (!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListApproveParkingByParkingIdRes>>
                    {
                        Message = "Danh sách trống.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListApproveParkingByParkingIdRes>>(lst);
                return new ServiceResponse<IEnumerable<GetListApproveParkingByParkingIdRes>>()
                {
                    Data = lstDto,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

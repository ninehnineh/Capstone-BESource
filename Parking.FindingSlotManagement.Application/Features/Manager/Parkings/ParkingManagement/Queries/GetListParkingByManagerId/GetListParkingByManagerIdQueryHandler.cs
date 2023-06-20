using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId
{
    public class GetListParkingByManagerIdQueryHandler : IRequestHandler<GetListParkingByManagerIdQuery, ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListParkingByManagerIdQueryHandler(IParkingRepository parkingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>> Handle(GetListParkingByManagerIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                var checkManagerExist = await _userRepository.GetById(request.ManagerId);
                if (checkManagerExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản quản lý.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (checkManagerExist.RoleId != 1)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>
                    {
                        Message = "tài khoản không phải là quản lý.",
                        StatusCode = 400,
                        Success = true
                    };
                }
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == checkManagerExist.UserId);
                if(businessExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var lst = await _parkingRepository.GetAllItemWithPagination(x => x.BusinessId == businessExist.BusinessProfileId, null, x => x.ParkingId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListParkingByManagerIdResponse>>(lst);
                if (lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                return new ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>
                {
                    Data = lstDto,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = lstDto.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

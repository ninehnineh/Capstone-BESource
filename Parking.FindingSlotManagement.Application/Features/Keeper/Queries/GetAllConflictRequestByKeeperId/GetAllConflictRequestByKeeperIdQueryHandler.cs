using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId
{
    public class GetAllConflictRequestByKeeperIdQueryHandler : IRequestHandler<GetAllConflictRequestByKeeperIdQuery, ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>>
    {
        private readonly IConflictRequestRepository _conflictRequestRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetAllConflictRequestByKeeperIdQueryHandler(IConflictRequestRepository conflictRequestRepository, IMapper mapper, IUserRepository userRepository)
        {
            _conflictRequestRepository = conflictRequestRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>> Handle(GetAllConflictRequestByKeeperIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetById(request.KeeperId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if(checkUserExist.RoleId != 2)
                {
                    return new ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>
                    {
                        Message = "Tài khoản của bạn không phải bảo vệ của bãi.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                if(checkUserExist.ParkingId == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>
                    {
                        Message = "Tài khoản của bạn không phải bảo vệ của bãi.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                var lst = await _conflictRequestRepository.GetAllItemWithPagination(x => x.ParkingId == checkUserExist.ParkingId, null, x => x.ConflictRequestId, true, request.PageNo, request.PageSize);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var lstDto = _mapper.Map<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
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

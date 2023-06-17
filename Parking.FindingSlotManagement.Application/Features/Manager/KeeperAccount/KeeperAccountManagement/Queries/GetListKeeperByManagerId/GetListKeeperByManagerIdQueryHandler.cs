﻿using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId
{
    public class GetListKeeperByManagerIdQueryHandler : IRequestHandler<GetListKeeperByManagerIdQuery, ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListKeeperByManagerIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>> Handle(GetListKeeperByManagerIdQuery request, CancellationToken cancellationToken)
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
                var lst = await _userRepository.GetAllItemWithPagination(x => x.ManagerId == request.ManagerId, null, x => x.UserId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListKeeperByManagerIdResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
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

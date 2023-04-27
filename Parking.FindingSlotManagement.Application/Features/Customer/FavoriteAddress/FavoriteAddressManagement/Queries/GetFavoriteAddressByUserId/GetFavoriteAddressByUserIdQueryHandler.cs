using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressByUserId
{
    public class GetFavoriteAddressByUserIdQueryHandler : IRequestHandler<GetFavoriteAddressByUserIdQuery, ServiceResponse<IEnumerable<GetFavoriteAddressByUserIdResponse>>>
    {
        private readonly IFavoriteAddressRepository _favoriteAddressRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetFavoriteAddressByUserIdQueryHandler(IFavoriteAddressRepository favoriteAddressRepository)
        {
            _favoriteAddressRepository = favoriteAddressRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetFavoriteAddressByUserIdResponse>>> Handle(GetFavoriteAddressByUserIdQuery request, CancellationToken cancellationToken)
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
                var lst = await _favoriteAddressRepository.GetAllItemWithPagination(x => x.UserId == request.UserId, null, null, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetFavoriteAddressByUserIdResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetFavoriteAddressByUserIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true,
                        Count = 0
                    };
                }
                return new ServiceResponse<IEnumerable<GetFavoriteAddressByUserIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
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

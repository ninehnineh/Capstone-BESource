using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressById
{
    public class GetFavoriteAddressByIdQueryHandler : IRequestHandler<GetFavoriteAddressByIdQuery, ServiceResponse<GetFavoriteAddressByIdResponse>>
    {
        private readonly IFavoriteAddressRepository _favoriteAddressRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetFavoriteAddressByIdQueryHandler(IFavoriteAddressRepository favoriteAddressRepository)
        {
            _favoriteAddressRepository = favoriteAddressRepository;
        }
        public async Task<ServiceResponse<GetFavoriteAddressByIdResponse>> Handle(GetFavoriteAddressByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var favoriteAddress = await _favoriteAddressRepository.GetById(request.FavoriteAddressId);
                if(favoriteAddress == null)
                {
                    return new ServiceResponse<GetFavoriteAddressByIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var favoriteAddressDto = _mapper.Map<GetFavoriteAddressByIdResponse>(favoriteAddress);
                return new ServiceResponse<GetFavoriteAddressByIdResponse>
                {
                    Message = "Thành công",
                    Data = favoriteAddressDto,
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

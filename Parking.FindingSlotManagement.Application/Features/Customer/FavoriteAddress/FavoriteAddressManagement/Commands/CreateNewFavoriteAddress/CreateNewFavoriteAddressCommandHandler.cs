using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress
{
    public class CreateNewFavoriteAddressCommandHandler : IRequestHandler<CreateNewFavoriteAddressCommand, ServiceResponse<int>>
    {
        private readonly IFavoriteAddressRepository _favoriteAddressRepository;
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewFavoriteAddressCommandHandler(IFavoriteAddressRepository favoriteAddressRepository, IAccountRepository accountRepository)
        {
            _favoriteAddressRepository = favoriteAddressRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewFavoriteAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _accountRepository.GetById(request.UserId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var favoriteAddressEntity = _mapper.Map<Domain.Entities.FavoriteAddress>(request);
                await _favoriteAddressRepository.Insert(favoriteAddressEntity);
                return new ServiceResponse<int>
                {
                    Data = favoriteAddressEntity.FavoriteAddressId,
                    Message = "Thành công",
                    StatusCode = 201,
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

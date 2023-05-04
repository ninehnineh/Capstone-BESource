using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.UpdateFavoriteAddress
{
    public class UpdateFavoriteAddressCommandHandler : IRequestHandler<UpdateFavoriteAddressCommand, ServiceResponse<string>>
    {
        private readonly IFavoriteAddressRepository _favoriteAddressRepository;

        public UpdateFavoriteAddressCommandHandler(IFavoriteAddressRepository favoriteAddressRepository)
        {
            _favoriteAddressRepository = favoriteAddressRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateFavoriteAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _favoriteAddressRepository.GetById(request.FavoriteAddressId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.TagName))
                {
                    checkExist.TagName = request.TagName;
                }
                if (!string.IsNullOrEmpty(request.Address))
                {
                    checkExist.Address = request.Address;
                }
                await _favoriteAddressRepository.Update(checkExist);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

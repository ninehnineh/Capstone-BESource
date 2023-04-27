using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.DeleteFavoriteAddress
{
    public class DeleteFavoriteAddressCommandHandler : IRequestHandler<DeleteFavoriteAddressCommand, ServiceResponse<string>>
    {
        private readonly IFavoriteAddressRepository _favoriteAddressRepository;

        public DeleteFavoriteAddressCommandHandler(IFavoriteAddressRepository favoriteAddressRepository)
        {
            _favoriteAddressRepository = favoriteAddressRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteFavoriteAddressCommand request, CancellationToken cancellationToken)
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
                await _favoriteAddressRepository.Delete(checkExist);
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

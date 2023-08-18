using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfBusinessAccount
{
    public class SumOfBusinessAccountQueryHandler : IRequestHandler<SumOfBusinessAccountQuery, ServiceResponse<SumOfBusinessAccountResponse>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IUserRepository _userRepository;

        public SumOfBusinessAccountQueryHandler(IBusinessProfileRepository businessProfileRepository, IParkingRepository parkingRepository, IUserRepository userRepository)
        {
            _businessProfileRepository = businessProfileRepository;
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<SumOfBusinessAccountResponse>> Handle(SumOfBusinessAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                SumOfBusinessAccountResponse resReturn = new();
                var bussiness = await _businessProfileRepository.GetAllItemWithCondition(null, null, null, true);
                if (!bussiness.Any())
                {
                    resReturn.NumberOfBusinessAccount = 0;
                }
                else
                {
                    resReturn.NumberOfBusinessAccount = bussiness.Count();
                }
                var parkings = await _parkingRepository.GetAllItemWithConditionByNoInclude(x => x.IsActive == true && x.IsAvailable == true);
                if (!parkings.Any())
                {
                    resReturn.NumberOfParkingActive = 0;
                }
                else
                {
                    resReturn.NumberOfParkingActive = parkings.Count();
                }
                var userUsingApp = await _userRepository.GetAllItemWithConditionByNoInclude(x => x.RoleId == 3 || x.RoleId == 2);
                if (!userUsingApp.Any())
                {
                    resReturn.NumberOfAccountUsingApp = 0;
                }
                else
                {
                    resReturn.NumberOfAccountUsingApp = userUsingApp.Count();
                }
                return new ServiceResponse<SumOfBusinessAccountResponse>
                {
                    Data = resReturn,
                    StatusCode = 200,
                    Success = true,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

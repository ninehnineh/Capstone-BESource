using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfCustomer
{
    public class SumOfCustomerQueryHandler : IRequestHandler<SumOfCustomerQuery, ServiceResponse<SumOfCustomerResponse>>
    {
        private readonly IUserRepository _userRepository;

        public SumOfCustomerQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<SumOfCustomerResponse>> Handle(SumOfCustomerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                SumOfCustomerResponse resReturn = new();
                var users = await _userRepository.GetAllItemWithConditionByNoInclude(x => x.RoleId == 3);
                if(!users.Any())
                {
                    resReturn.NumberOfCustomer = 0;
                }
                else
                {
                    resReturn.NumberOfCustomer = users.Count();
                }
                return new ServiceResponse<SumOfCustomerResponse>
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

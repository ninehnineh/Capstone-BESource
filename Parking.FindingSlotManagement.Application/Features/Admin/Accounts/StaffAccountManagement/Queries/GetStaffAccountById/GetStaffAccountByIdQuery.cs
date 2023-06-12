using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetStaffAccountById
{
    public class GetStaffAccountByIdQuery : IRequest<ServiceResponse<GetStaffAccountByIdResponse>>
    {
        public int UserId { get; set; }
    }
}

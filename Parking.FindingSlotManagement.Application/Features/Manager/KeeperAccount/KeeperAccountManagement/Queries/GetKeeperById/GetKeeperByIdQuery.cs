using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetKeeperById
{
    public class GetKeeperByIdQuery : IRequest<ServiceResponse<GetKeeperByIdResponse>>
    {
        public int UserId { get; set; }
    }
}

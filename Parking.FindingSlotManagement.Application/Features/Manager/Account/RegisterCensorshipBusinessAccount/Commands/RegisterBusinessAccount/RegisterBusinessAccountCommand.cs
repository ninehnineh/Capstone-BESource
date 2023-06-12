using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount
{
    public class RegisterBusinessAccountCommand : IRequest<ServiceResponse<int>>
    {
        public UserEntity UserEntity { get; set; }
        public BusinessProfileEntity BusinessProfileEntity { get; set; }
    }
}

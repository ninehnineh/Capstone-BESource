using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.VerifyEmail.Queries.VerifyEmailExist
{
    public class VerifyEmailExistQuery : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
    }
}

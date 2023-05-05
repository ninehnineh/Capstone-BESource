using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IStaffAuthenticationRepository
    {
        Task<ServiceResponse<AuthResponse>> StaffLogin(AuthRequest request);
    }
}

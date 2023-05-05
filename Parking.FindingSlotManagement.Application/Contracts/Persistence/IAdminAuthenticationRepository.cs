using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IAdminAuthenticationRepository
    {
        Task<ServiceResponse<AuthResponse>> AdminLogin(AuthRequest request);
        Task<ServiceResponse<string>> AdminRegister(User user, string password);
    }
}

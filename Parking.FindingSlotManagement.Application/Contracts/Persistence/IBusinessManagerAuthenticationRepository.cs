using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IBusinessManagerAuthenticationRepository
    {
        Task<ServiceResponse<AuthResponse>> ManagerLogin(AuthRequest request);
        Task<ServiceResponse<string>> ManagerRegister(User user, string password);
    }
}

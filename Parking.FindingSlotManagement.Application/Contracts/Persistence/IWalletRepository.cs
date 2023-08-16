using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<string> UpdateMoneyInWallet(Wallet wallet, string? status);
        Wallet GetWallet(int userId);
        Task<Wallet> GetWalletById(int id);
    }
}

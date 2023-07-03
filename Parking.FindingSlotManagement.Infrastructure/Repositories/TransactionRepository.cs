using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ParkZDbContext _dbContext;

        public TransactionRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateNewTransactionWithDeposit(Transaction transaction)
        {
            try
            {
                transaction.PaymentMethod = Domain.Enum.PaymentMethod.thanh_toan_online.ToString();
                _dbContext.Transactions.Add(transaction);
                await _dbContext.SaveChangesAsync();
                return transaction.TransactionId;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

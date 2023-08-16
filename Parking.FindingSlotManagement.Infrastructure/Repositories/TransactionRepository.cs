using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
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

        public async Task ChangeTransactionStatusByBookingId(List<int> bookingids, string reason)
        {
            try
            {
                foreach (var id in bookingids)
                {
                    var transactions = await _dbContext.Transactions
                        .FirstOrDefaultAsync(x => x.BookingId == id);

                    transactions.Status = BookingPaymentStatus.Huy.ToString();
                    transactions.Description = reason;
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Error at ChangeTransactionStatusByBookingId: Message " + ex.Message);
            }
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

        public async Task<List<Transaction>> GetPrePaidTransactions(List<BookingDetails> bookingDetails)
        {
            try
            {
                List<Transaction> result = new List<Transaction>();

                foreach (var bookingDetail in bookingDetails)
                {
                    var prePaidTransaction = await _dbContext.Transactions
                        .Include(x => x.Wallet)
                        .FirstOrDefaultAsync(x => x.BookingId == bookingDetail.BookingId &&
                                                    x.PaymentMethod!.Equals(PaymentMethod.tra_truoc.ToString()) &&
                                                    x.Status!.Equals(BookingPaymentStatus.Da_thanh_toan.ToString()));
                    
                    if (prePaidTransaction != null)
                        result.Add(prePaidTransaction!);
                }

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Error at GetPrePaidTransactions: Message " + ex.Message);
            }
        }

        public async Task ChangeStatusOriginalTransactionsByBookingDetail(List<BookingDetails> bookingDetails, string reason)
        {
            try
            {
                foreach (var bookingDetail in bookingDetails)
                {
                    var bookedTransactions = await _dbContext.Transactions
                        .FirstOrDefaultAsync(x => x.BookingId == bookingDetail.BookingId);

                    bookedTransactions.Status = BookingPaymentStatus.Huy.ToString();
                    bookedTransactions.Description = $"{reason}";
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error at GetPaymentMethod: Message " + ex.Message);
            }
        }
    }
}

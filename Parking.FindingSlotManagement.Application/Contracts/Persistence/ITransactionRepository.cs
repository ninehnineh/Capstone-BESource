using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<int> CreateNewTransactionWithDeposit(Transaction transaction);
        Task ChangeStatusOriginalTransactionsByBookingDetail(List<BookingDetails> bookingDetails, string reason);
        Task ChangeTransactionStatusByBookingId(List<int> bookingid, string reason);
        Task<List<Transaction>> GetPrePaidTransactions(List<BookingDetails> bookingDetails);
    }
}

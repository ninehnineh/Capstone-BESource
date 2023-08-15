using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly ParkZDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public WalletRepository(ParkZDbContext dbContext, IMapper mapper, ITransactionRepository transactionRepository) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public Wallet GetWallet(int userId)
        {
            try
            {
                var extityExist = _dbContext.Wallets.FirstOrDefault(x => x.UserId == userId);
                if (extityExist != null)
                {
                    return extityExist;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Wallet> GetWalletById(int id)
        {
            try
            {
                var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == id);
                return wallet;
            }
            catch (Exception ex)
            {

                throw new Exception("Error at GetWalletById: Message "+ ex.Message);
            }
        }

        public async Task<string> UpdateMoneyInWallet(Wallet wallet, string? status)
        {
            try
            {
                var checkUserExistInWallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == wallet.UserId);
                if (checkUserExistInWallet == null)
                {
                    var walletAdd = new Wallet
                    {
                        Balance = wallet.Balance,
                        Debt = 0,
                        UserId = wallet.UserId,
                    };
                    _dbContext.Wallets.Add(walletAdd);
                    await _dbContext.SaveChangesAsync();
                    if (status == null)
                    {
                        var transactionEntityCommand = new CreateNewTransactionCommand
                        {
                            Price = wallet.Balance,
                            WalletId = walletAdd.WalletId,
                            Status = Domain.Enum.TransactionStatus.Nap_tien_vao_vi_khong_thanh_cong.ToString(),
                            Description = "Nạp tiền vào ví."
                        };
                        var entityToAdd = _mapper.Map<Transaction>(transactionEntityCommand);
                        entityToAdd.CreatedDate = DateTime.UtcNow.AddHours(7);
                        await _transactionRepository.CreateNewTransactionWithDeposit(entityToAdd);
                        return "Thành công";
                    }
                    var transactionEntityCommand2 = new CreateNewTransactionCommand
                    {
                        Price = wallet.Balance,
                        WalletId = walletAdd.WalletId,
                        Status = Domain.Enum.TransactionStatus.Nap_tien_vao_vi_thanh_cong.ToString(),
                        Description = "Nạp tiền vào ví."
                    };
                    var entityToAdd2 = _mapper.Map<Transaction>(transactionEntityCommand2);
                    entityToAdd2.CreatedDate = DateTime.UtcNow.AddHours(7);
                    await _transactionRepository.CreateNewTransactionWithDeposit(entityToAdd2);
                    return "Thành công";
                }
                if (checkUserExistInWallet.Debt > 0)
                {
                    checkUserExistInWallet.Debt = wallet.Balance - checkUserExistInWallet.Debt;
                    if (checkUserExistInWallet.Debt >= 0)
                    {
                        checkUserExistInWallet.Balance = (decimal)checkUserExistInWallet.Debt;
                        checkUserExistInWallet.Debt = 0;
                    }
                    else if (checkUserExistInWallet.Debt < 0)
                    {
                        checkUserExistInWallet.Balance = 0;
                        checkUserExistInWallet.Debt = -(checkUserExistInWallet.Debt);
                    }

                }
                else
                {
                    checkUserExistInWallet.Balance += wallet.Balance;
                }
                await _dbContext.SaveChangesAsync();
                if (status == null)
                {
                    var transactionEntityCommand = new CreateNewTransactionCommand
                    {
                        Price = wallet.Balance,
                        WalletId = checkUserExistInWallet.WalletId,
                        Status = Domain.Enum.TransactionStatus.Nap_tien_vao_vi_khong_thanh_cong.ToString(),
                        Description = "Nạp tiền vào ví."
                    };
                    var entityToAdd = _mapper.Map<Transaction>(transactionEntityCommand);
                    entityToAdd.CreatedDate = DateTime.UtcNow.AddHours(7);
                    await _transactionRepository.CreateNewTransactionWithDeposit(entityToAdd);
                    return "Thành công";
                }
                var transactionEntityCommand3 = new CreateNewTransactionCommand
                {
                    Price = wallet.Balance,
                    WalletId = checkUserExistInWallet.WalletId,
                    Status = Domain.Enum.TransactionStatus.Nap_tien_vao_vi_thanh_cong.ToString(),
                    Description = "Nạp tiền vào ví."
                };
                var entityToAdd3 = _mapper.Map<Transaction>(transactionEntityCommand3);
                entityToAdd3.CreatedDate = DateTime.UtcNow.AddHours(7);
                await _transactionRepository.CreateNewTransactionWithDeposit(entityToAdd3);
                return "Thành công";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

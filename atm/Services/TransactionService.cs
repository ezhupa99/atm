using System;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models;
using atm.Repositories;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ILogger<TransactionService> logger,
            ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task<int> Create(int accountId, int atmId, decimal amount, string currentAllocation,
            string bestAllocation)
        {
            try
            {
                var newTransaction = new Transaction
                {
                    AccountId = accountId,
                    AtmId = atmId,
                    Amount = amount,
                    CurrentAllocation = currentAllocation,
                    BestAllocation = bestAllocation
                };

                await _transactionRepository.AddAsync(newTransaction);
                await _transactionRepository.SaveChangesAsync();

                return newTransaction.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Repositories;
using atm.Models;
using atm.Models.ViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountHistoryService _accountHistoryService;

        public AccountService(ILogger<AccountService> logger,
            IAccountRepository accountRepository, IAccountHistoryService accountHistoryService)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _accountHistoryService = accountHistoryService;
        }

        public async Task<int> Create(int userId)
        {
            try
            {
                var newAccount = new Account
                {
                    UserId = userId,
                };

                await _accountRepository.AddAsync(newAccount);
                await _accountRepository.SaveChangesAsync();

                var newAccountId = newAccount.Id;

                await _accountHistoryService.Create(newAccountId);

                return newAccountId;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<bool> UpdateBalance(AccountDto account, int id, bool isWithdraw)
        {
            try
            {
                var accountToUpdate = await _accountRepository.GetByIdAsync(id);

                if (isWithdraw && accountToUpdate.Balance < account.Balance)
                {
                    return false;
                }

                var balance = !isWithdraw ? account.Balance : account.Balance * -1;

                accountToUpdate.Balance += balance;

                var newAccountHistory = new AccountHistory
                {
                    AccountId = id,
                    Amount = balance
                };

                accountToUpdate.Histories.Add(newAccountHistory);

                await _accountRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<List<AccountDto>> GetAll()
        {
            try
            {
                // check if admin

                var accounts = await _accountRepository.GetAllAsyncNonTracking();
                return accounts.Select(account => account.Adapt<AccountDto>()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<AccountDto> GetAccountById(int id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);

                return account.Adapt<AccountDto>();
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<List<AccountDto>> GetAccountsForUser(int userId)
        {
            try
            {
                // supposedly we get the user id from the token

                var account = (await _accountRepository.GetAllAsyncNonTracking())
                    .Where(account => account.UserId == userId).Select(account => account.Adapt(new AccountDto()))
                    .ToList();

                return account;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        // public async Task 
    }
}
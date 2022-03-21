using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models;
using atm.Models.ViewModels;
using atm.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class AccountHistoryService : IAccountHistoryService
    {
        private readonly ILogger<AccountHistoryService> _logger;
        private readonly IAccountHistoryRepository _accountHistoryRepository;

        public AccountHistoryService(ILogger<AccountHistoryService> logger,
            IAccountHistoryRepository accountHistoryRepository)
        {
            _logger = logger;
            _accountHistoryRepository = accountHistoryRepository;
        }

        public async Task<int> Create(int accountId)
        {
            try
            {
                var newAccountHistory = new AccountHistory
                {
                    AccountId = accountId,
                    Amount = 0
                };

                await _accountHistoryRepository.AddAsync(newAccountHistory);
                await _accountHistoryRepository.SaveChangesAsync();

                return newAccountHistory.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<AccountHistoryDto>> GetHistoryForAccountId(int accountId)
        {
            try
            {
                TypeAdapterConfig<AccountHistory, AccountHistoryDto>
                    .NewConfig()
                    .Map(dest => dest.Date,
                        src => src.Created);

                var histories = await _accountHistoryRepository.CustomQueryNonTracking()
                    .Where(history => history.AccountId == accountId)
                    .OrderBy(q => q.Created)
                    .ToListAsync();

                return histories.Adapt(new List<AccountHistoryDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
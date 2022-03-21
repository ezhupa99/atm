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
using Newtonsoft.Json;

namespace atm.Services
{
    public class AtmService : IAtmService
    {
        private readonly ILogger<AtmService> _logger;
        private readonly IATMRepository _atmRepository;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        private const int MinSumOnAtm = 5000;

        public AtmService(ILogger<AtmService> logger,
            IATMRepository atmRepository, IAccountService accountService, ITransactionService transactionService)
        {
            _logger = logger;
            _atmRepository = atmRepository;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        public async Task<int> Create(AtmDto atm)
        {
            try
            {
                var newAtm = new ATM()
                {
                    Banknote500 = atm.Banknote500,
                    Banknote1000 = atm.Banknote1000,
                    Banknote2000 = atm.Banknote2000,
                    Banknote5000 = atm.Banknote5000,
                };

                await _atmRepository.AddAsync(newAtm);
                await _atmRepository.SaveChangesAsync();

                return newAtm.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<bool> Update(AtmDto atm)
        {
            try
            {
                var atmToUpdate = await _atmRepository.GetByIdAsync(atm.Id);

                atm.Adapt(atmToUpdate);
                await _atmRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return false;
            }
        }

        public async Task<string> Withdraw(int atmId, int accountId, decimal amount)
        {
            try
            {
                if (amount % 5 != 0)
                    throw new Exception("Amount must be multiple of 5");

                var atm = await _atmRepository.GetByIdAsync(atmId);

                if (atm == null)
                    throw new Exception("ATM not found");

                var totalSumOnAtm = TotalSumOfBanknotes(atm);

                if (totalSumOnAtm <= MinSumOnAtm)
                    throw new Exception("ATM is almost empty");

                if (amount > totalSumOnAtm)
                    throw new Exception("ATM has not enough money");

                var account = await _accountService.GetAccountById(accountId);

                if (account == null)
                    throw new Exception("Account not found");

                if (account.Balance < amount)
                    throw new Exception("Not enough money on account");

                CalculateBestCaseAllocationOfNotes2(amount, out var bestAllocation);

                CalculateCurrentAtmAllocationOfNotes2(amount, atm, out var currentAllocation);

                atm.Banknote5000 -= currentAllocation[5000];
                atm.Banknote2000 -= currentAllocation[2000];
                atm.Banknote1000 -= currentAllocation[1000];
                atm.Banknote500 -= currentAllocation[500];

                await _atmRepository.SaveChangesAsync();

                var accountDto = new AccountDto
                {
                    Balance = amount
                };

                await _accountService.UpdateBalance(accountDto, accountId, true);

                await _transactionService.Create(accountId, atmId, amount,
                    JsonConvert.SerializeObject(currentAllocation), JsonConvert.SerializeObject(bestAllocation));

                return JsonConvert.SerializeObject(currentAllocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void CalculateBestCaseAllocationOfNotes(decimal amount, out Dictionary<int, int> bestAllocation)
        {
            // key banknote value and value is how many times hit
            bestAllocation = new Dictionary<int, int>();

            while (amount > 0)
            {
                for (var i = 1; amount > 0; i++)
                {
                    if (amount - 5000 < 0)
                    {
                        break;
                    }

                    amount -= 5000;
                    bestAllocation[5000] = i;
                }

                for (var i = 1; amount > 0; i++)
                {
                    if (amount - 2000 < 0)
                    {
                        break;
                    }

                    amount -= 2000;
                    bestAllocation[2000] = i;
                }

                for (var i = 1; amount > 0; i++)
                {
                    if (amount - 1000 < 0)
                    {
                        break;
                    }

                    amount -= 1000;
                    bestAllocation[1000] = i;
                }

                for (var i = 1; amount > 0; i++)
                {
                    if (amount - 500 < 0)
                    {
                        break;
                    }

                    amount -= 500;
                    bestAllocation[500] = i;
                }
            }

            if (bestAllocation.Values.Sum() > 20)
                throw new Exception("20 banknote limit exceeded");
        }

        private static void CalculateBestCaseAllocationOfNotes2(decimal amount, out Dictionary<int, int> bestAllocation)
        {
            // key banknote value and value is how many times hit
            bestAllocation = new Dictionary<int, int>();

            var difference = amount % 5000;
            bestAllocation[5000] = (int) ((amount - difference) / 5000);
            amount = difference;

            difference %= 2000;
            bestAllocation[2000] = (int) ((amount - difference) / 2000);
            amount = difference;

            difference %= 1000;
            bestAllocation[1000] = (int) ((amount - difference) / 1000);
            amount = difference;

            difference %= 500;
            bestAllocation[500] = (int) ((amount - difference) / 500);

            if (bestAllocation.Values.Sum() > 20)
                throw new Exception("20 banknote limit exceeded");
        }

        private static void CalculateCurrentAtmAllocationOfNotes2(decimal amount, ATM atm,
            out Dictionary<int, int> currentAllocation)
        {
            // key banknote value and value is how many times hit
            currentAllocation = new Dictionary<int, int>();

            var difference = amount % 5000;
            var currentCount = (int) ((amount - difference) / 5000);

            if (currentCount > GetAtm5000BankNotes(atm))
            {
                currentCount = GetAtm5000BankNotes(atm);
                difference = amount - (5000 * currentCount);
            }

            currentAllocation[5000] = currentCount;
            amount = difference;

            difference = amount % 2000;
            currentCount = (int) ((amount - difference) / 2000);

            if (currentCount > GetAtm2000BankNotes(atm))
            {
                currentCount = GetAtm2000BankNotes(atm);
                difference = amount - (2000 * currentCount);
            }

            currentAllocation[2000] = currentCount;
            amount = difference;

            difference = amount % 1000;
            currentCount = (int) ((amount - difference) / 1000);

            if (currentCount > GetAtm1000BankNotes(atm))
            {
                currentCount = GetAtm1000BankNotes(atm);
                difference = amount - (1000 * currentCount);
            }

            currentAllocation[1000] = currentCount;
            amount = difference;

            difference = amount % 500;
            currentCount = (int) ((amount - difference) / 500);

            if (currentCount > GetAtm500BankNotes(atm))
            {
                currentCount = GetAtm500BankNotes(atm);
                difference = amount - (500 * currentCount);
            }

            currentAllocation[500] = currentCount;
            amount = difference;

            if (currentAllocation.Values.Sum() > 20)
                throw new Exception("20 banknote limit exceeded");
        }

        private static decimal TotalSumOfBanknotes(ATM atm)
        {
            return atm.Banknote500 * 500 + atm.Banknote1000 * 1000 + atm.Banknote2000 * 2000 + atm.Banknote5000 * 5000;
        }

        private static int GetAtm5000BankNotes(ATM atm)
        {
            return atm.Banknote5000;
        }

        private static int GetAtm2000BankNotes(ATM atm)
        {
            return atm.Banknote2000;
        }

        private static int GetAtm1000BankNotes(ATM atm)
        {
            return atm.Banknote1000;
        }

        private static int GetAtm500BankNotes(ATM atm)
        {
            return atm.Banknote500;
        }
    }
}
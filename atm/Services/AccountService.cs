using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Repositories;
using atm.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountService(ILogger<AccountService> logger,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }
    }
}
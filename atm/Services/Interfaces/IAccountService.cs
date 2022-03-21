using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Models.ViewModels;

namespace atm.Interfaces
{
    public interface IAccountService
    {
        public Task<int> Create(int userId);
        public Task<bool> UpdateBalance(AccountDto account, int id, bool isWithdraw);

        public Task<List<AccountDto>> GetAll();

        public Task<AccountDto> GetAccountById(int id);

        public Task<List<AccountDto>> GetAccountsForUser(int userId);
    }
}
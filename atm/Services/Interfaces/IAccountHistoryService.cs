using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Models;
using atm.Models.ViewModels;

namespace atm.Interfaces
{
    public interface IAccountHistoryService
    {
        public Task<int> Create(int accountId);

        public Task<List<AccountHistoryDto>> GetHistoryForAccountId(int accountId);
    }
}
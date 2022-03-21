using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Models.ViewModels;

namespace atm.Interfaces
{
    public interface IAtmService
    {
        public Task<int> Create(AtmDto atm);

        public Task<bool> Update(AtmDto atm);

        public Task<string> Withdraw(int atmId, int accountId, decimal amount);
    }
}
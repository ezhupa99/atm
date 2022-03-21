using System.Threading.Tasks;

namespace atm.Interfaces
{
    public interface ITransactionService
    {
        public Task<int> Create(int accountId, int atmId, decimal amount, string currentAllocation,
            string bestAllocation);
    }
}
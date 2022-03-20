using atm.Models;
using atm.Repositories.BaseRepository;

namespace atm.Repositories
{
    public interface IAccountRepository : IBaseModelRepository<Account>
    {
    }

    public interface IAccountHistoryRepository : IBaseModelRepository<AccountHistory>
    {
    }

    public interface IATMRepository : IBaseModelRepository<ATM>
    {
    }

    public interface IRoleRepository : IBaseModelRepository<Role>
    {
    }

    public interface IUserRepository : IBaseModelRepository<User>
    {
    }
}
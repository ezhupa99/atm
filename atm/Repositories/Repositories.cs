using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atm.Data;
using atm.Models;
using atm.Repositories.BaseRepository;

namespace atm.Repositories
{
    public class AccountRepository : BaseModelModelRepository<Account>, IAccountRepository
    {
        public AccountRepository(ComprogContext context)
            : base(context)
        {
        }
    }

    public class AccountHistoryRepository : BaseModelModelRepository<AccountHistory>,
        IAccountHistoryRepository
    {
        public AccountHistoryRepository(ComprogContext context)
            : base(context)
        {
        }
    }

    public class ATMRepository : BaseModelModelRepository<ATM>, IATMRepository
    {
        public ATMRepository(ComprogContext context)
            : base(context)
        {
        }
    }

    public class RoleRepository : BaseModelModelRepository<Role>, IRoleRepository
    {
        public RoleRepository(ComprogContext context)
            : base(context)
        {
        }
    }

    public class UserRepository : BaseModelModelRepository<User>, IUserRepository
    {
        public UserRepository(ComprogContext context)
            : base(context)
        {
        }
    }
}
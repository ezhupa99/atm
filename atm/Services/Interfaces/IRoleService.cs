using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Models;

namespace atm.Interfaces
{
    public interface IRoleService
    {
        public Task<IEnumerable<Role>> GetAllRolesNT();

        public Task<IEnumerable<Role>> GetAllRoles();

        public Task<Role> GetAdminRole();

        public Task<Role> GetUserRole();
    }
}
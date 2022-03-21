using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models;
using atm.Repositories;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepository _roleRepository;

        private const string AdminRoleName = "Admin";
        private const string UserRoleName = "User";

        public RoleService(ILogger<RoleService> logger,
            IRoleRepository roleRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetAllRolesNT()
        {
            return await _roleRepository.GetAllAsyncNonTracking();
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role> GetAdminRole()
        {
            return (await GetAllRolesNT()).FirstOrDefault(role => role.Name == AdminRoleName);
        }

        public async Task<Role> GetUserRole()
        {
            return (await GetAllRolesNT()).FirstOrDefault(role => role.Name == UserRoleName);
        }
    }
}
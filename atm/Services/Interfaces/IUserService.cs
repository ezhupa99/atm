using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Models;
using atm.Models.ViewModels;

namespace atm.Interfaces
{
    public interface IUserService
    {
        public Task<int> CreateUser(UsersDto users);

        public Task<int> CreateAdmin(UsersDto users);

        public Task<List<UsersDto>> GetAllUsers();

        public Task<UsersDto> GetUserById(int id);
    }
}
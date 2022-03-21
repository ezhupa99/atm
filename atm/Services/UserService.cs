using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models;
using atm.Models.ViewModels;
using atm.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace atm.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;

        public UserService(ILogger<UserService> logger,
            IUserRepository userRepository, IRoleService roleService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _roleService = roleService;
        }

        public async Task<int> CreateUser(UsersDto users)
        {
            // check if user role Admin
            try
            {
                var newUser = new User();
                var userRole = await _roleService.GetUserRole();
                newUser.RoleId = userRole.Id;
                newUser.Name = users.Name;

                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveChangesAsync();

                return newUser.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        // to keep the application going at the beginning
        public async Task<int> CreateAdmin(UsersDto users)
        {
            try
            {
                var newAdmin = new User();
                var adminRole = await _roleService.GetAdminRole();
                newAdmin.RoleId = adminRole.Id;
                newAdmin.Name = users.Name;

                await _userRepository.AddAsync(newAdmin);
                await _userRepository.SaveChangesAsync();

                return newAdmin.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<List<UsersDto>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsyncNonTracking();

                return users.Select(user => user.Adapt(new UsersDto())).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }

        public async Task<UsersDto> GetUserById(int id)
        {
            try
            {
                TypeAdapterConfig<User, UserDto>
                    .NewConfig()
                    .Map(dest => dest.RoleName,
                        src => src.Role.Name);

                var user = await _userRepository.CustomQueryNonTracking()
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                return user.Adapt(new UserDto());
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                throw;
            }
        }
    }
}
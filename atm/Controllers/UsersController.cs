using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace atm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IAccountHistoryService _accountHistoryService;

        public UsersController(ILogger<UsersController> logger, IUserService userService,
            IAccountService accountService, IAccountHistoryService accountHistoryService)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _accountHistoryService = accountHistoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsersDto>>> Get()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<List<UsersDto>>> Get(int userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] UsersDto users)
        {
            try
            {
                var newUserId = await _userService.CreateUser(users);

                return Ok(newUserId);
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpPost("createAdmin")]
        public async Task<IActionResult> CreateAdmin(UsersDto admin)
        {
            try
            {
                var newAdminId = await _userService.CreateAdmin(admin);

                return Ok(newAdminId);
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{userId:int}/accounts")]
        public async Task<ActionResult<List<UsersDto>>> GetAccountsForUser(int userId)
        {
            try
            {
                var accounts = await _accountService.GetAccountsForUser(userId);

                return Ok(accounts);
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpPost("{userId:int}/accounts")]
        public async Task<ActionResult<int>> CreateAccountForUser(int userId)
        {
            try
            {
                var accounts = await _accountService.Create(userId);

                return Ok(accounts);
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpPut("accounts/{accountId:int}/deposit")]
        public async Task<IActionResult> DepositToBalance([FromBody] AccountDto account, int accountId)
        {
            try
            {
                // check if admin
                var result = await _accountService.UpdateBalance(account, accountId, false);

                if (!result)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("accounts/{accountId:int}/withdraw")]
        public async Task<IActionResult> WithdrawFromBalance([FromBody] AccountDto account, int accountId)
        {
            try
            {
                // check if admin
                var result = await _accountService.UpdateBalance(account, accountId, true);

                if (!result)
                    return BadRequest("You don't have enough money to withdraw");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("accounts/{accountId:int}/balance")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            try
            {
                // check if admin
                var result = await _accountService.GetAccountById(accountId);

                return Ok(result.Balance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("accounts/{accountId:int}/histories")]
        public async Task<IActionResult> GetHistories(int accountId)
        {
            try
            {
                // check if admin
                var result = await _accountHistoryService.GetHistoryForAccountId(accountId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
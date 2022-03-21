using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using atm.Interfaces;
using atm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace atm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtmsController : ControllerBase
    {
        private readonly ILogger<AtmsController> _logger;
        private readonly IAtmService _atmService;

        public AtmsController(ILogger<AtmsController> logger, IAtmService atmService)
        {
            _logger = logger;
            _atmService = atmService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAtm(AtmDto atm)
        {
            try
            {
                var newAtmId = await _atmService.Create(atm);

                return Ok(newAtmId);
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAtm([FromBody] AtmDto atm)
        {
            try
            {
                var result = await _atmService.Update(atm);

                if (!result)
                    return BadRequest();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return BadRequest();
            }
        }

        [HttpPost("{atmId:int}/withdraw/{accountId:int}")]
        public async Task<IActionResult> WithdrawFromAtm(int atmId, int accountId,
            [Required] decimal amount)
        {
            try
            {
                await _atmService.Withdraw(atmId, accountId, amount);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }
    }
}
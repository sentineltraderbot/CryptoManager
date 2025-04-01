using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManager.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SentinelTraderBotTokenController : BaseController
    {
        private readonly ISentinelTraderBotTokenService _service;
        private readonly IAccountService _accountService;
        public SentinelTraderBotTokenController(
            ISentinelTraderBotTokenService service,
            IAccountService accountService,
            IMapper mapper) : base(mapper)
        {
            _service = service;
            _accountService = accountService;
        }

         /// <summary>
        /// Get all balances from a wallet
        /// </summary>
        /// <returns>All Balances</returns>
        /// <response code="200">if success</response>
        /// <response code="400">if error</response>
        /// <response code="500">if internal error</response>
        [HttpGet("balances")]
        [ProducesResponseType(typeof(ObjectResult<TickerBalanceDTO[]>), 200)]
        public async Task<IActionResult> GetBalancesAsync([FromQuery] string solanaWalletAddress)
        {
            return Ok(await _service.GetBalancesAsync(solanaWalletAddress));
        }

        /// <summary>
        /// Save an order in database
        /// </summary>
        /// <param name="entity">order to save in database</param>
        /// <response code="200">if success</response>
        [HttpPost("airdrop")]
        [ProducesResponseType(typeof(SimpleObjectResult), 200)]
        public async Task<IActionResult> AirdropAsync([FromQuery] string solanaWalletAddress)
        {
            var response = await _service.SentinelTraderTokenAirdropAsync(solanaWalletAddress, GetUserId());

            if(response.HasSucceded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}

using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITokenSaleRepository _tokenSaleRepository;

        public SentinelTraderBotTokenController(
            ISentinelTraderBotTokenService service,
            ITokenSaleRepository tokenSaleRepository,
            IMapper mapper) : base(mapper)
        {
            _service = service;
            _tokenSaleRepository = tokenSaleRepository;
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
        /// Save Token Sale in database
        /// </summary>
        /// <param name="entity">Asset to save in database</param>
        /// <response code="200">if success</response>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleObjectResult), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]TokenSaleDTO entity)
        {
            await _tokenSaleRepository.InsertAsync(_mapper.Map<TokenSale>(entity));
            return Ok(SimpleObjectResult.Success());
        }
    }
}

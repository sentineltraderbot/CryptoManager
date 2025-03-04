using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManager.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : BaseController
    {
        private readonly IExchangeIntegrationStrategyContext _exchangeIntegrationStrategyContext;
        public TickerController(
            IExchangeIntegrationStrategyContext exchangeIntegrationStrategyContext,
            IMapper mapper) : base(mapper)
        {
            _exchangeIntegrationStrategyContext = exchangeIntegrationStrategyContext;
        }

        /// <summary>
        /// Get all tickers from an exchange
        /// </summary>
        /// <param name="baseAssetSymbol"></param>
        /// <param name="quoteAssetSymbol"></param>
        /// <param name="exchangesIntegratedType"></param>
        /// <returns>Current Price</returns>
        /// <response code="200">if success</response>
        /// <response code="400">if error</response>
        /// <response code="500">if internal error</response>
        /// <response code="404">if not found</response>
        /// <response code="401">if unauthorized</response>
        [HttpGet("GetCurrentPrice")]
        [ProducesResponseType(typeof(ObjectResult<TickerPriceDTO>), 200)]
        public async Task<IActionResult> GetCurrentPriceAsync([FromQuery] string baseAssetSymbol, [FromQuery] string quoteAssetSymbol,[FromQuery] ExchangesIntegratedType exchangesIntegratedType)
        {
            return Ok(await _exchangeIntegrationStrategyContext.GetCurrentPriceAsync(baseAssetSymbol, quoteAssetSymbol, exchangesIntegratedType));
        }

        /// <summary>
        /// Get all tickers from an exchange
        /// </summary>
        /// <param name="exchangesIntegratedType"></param>
        /// <returns>All Prices</returns>
        /// <response code="200">if success</response>
        /// <response code="400">if error</response>
        /// <response code="500">if internal error</response>
        [HttpGet]
        [ProducesResponseType(typeof(TickerPriceDTO[]), 200)]
        public async Task<IActionResult> GetTickersAsync([FromQuery] ExchangesIntegratedType exchangesIntegratedType)
        {
            return Ok(await _exchangeIntegrationStrategyContext.GetTickersAsync(exchangesIntegratedType));
        }
    }
}

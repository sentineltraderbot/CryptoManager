using System;
using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManager.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/order")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderService _business;

        public OrderController(IOrderRepository repository,
                               IOrderService business,
                               IMapper mapper) : base(mapper)
        {
            _repository = repository;
            _business = business;
        }

        /// <summary>
        /// gets all orders by logged user from database
        /// </summary>
        /// <returns>list of all orders</returns>
        /// <response code="200">if success</response>
        [HttpGet]
        [Route("GetOrderDetailsByApplicationUser")]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public async Task<IActionResult> GetOrderDetailsByApplicationUser([FromQuery] GetOrdersCriteria getOrdersCriteria)
        {
            getOrdersCriteria.ApplicationUserId = GetUserId();
            return Ok(await _business.GetOrdersDetailsByApplicationUserAsync(getOrdersCriteria));
        }

        /// <summary>
        /// Save an order in database
        /// </summary>
        /// <param name="entity">order to save in database</param>
        /// <response code="200">if success</response>
        [HttpPost]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public async Task<IActionResult> Post([FromBody] OrderDTO entity)
        {
            var order = _mapper.Map<Order>(entity);
            order.ApplicationUserId = order.ApplicationUserId == Guid.Empty ? GetUserId() : order.ApplicationUserId;
            return Ok(_mapper.Map<OrderDTO>(await _business.CreateOrderAsync(order)));
        }

        /// <summary>
        /// update an order in database
        /// </summary>
        /// <param name="entity">Order to update</param>
        /// <response code="200">if success</response>
        [HttpPut]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public Task<IActionResult> Put([FromBody] OrderDTO entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete an order from database
        /// </summary>
        /// <param name="id">id of an order to delete</param>
        /// <response code="404">if parameter don't match with an order</response>
        /// <response code="200">if success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ObjectResult), 404)]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(entity);
            return Ok();
        }
    }
}

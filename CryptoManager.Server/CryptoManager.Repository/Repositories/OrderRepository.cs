using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using CryptoManager.Repository.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoManager.Repository.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderRepository(IORM<Order> orm, IOrderItemRepository orderItemRepository) : base(orm)
        {
            _orderItemRepository = orderItemRepository;
        }

        public override async Task<Order> InsertAsync(Order entity)
        {
            var orderItens = entity.OrderItems;
            entity.OrderItems = new List<OrderItem>();
            entity = await base.InsertAsync(entity);

            if (orderItens != null)
            {
                foreach (var orderItem in orderItens)
                {
                    orderItem.OrderId = entity.Id;
                    entity.OrderItems.Add(await _orderItemRepository.InsertAsync(orderItem));
                }
            }

            return entity;
        }

        public override async Task DeleteAsync(Order entity)
        {
            var orderItems = await _orderItemRepository.GetAllByOrderIdAsync(entity.Id);
            foreach (var orderItem in orderItems)
            {
                await _orderItemRepository.DeleteAsync(orderItem);
            }
            await base.DeleteAsync(entity);
        }

        public Task<List<Order>> GetAllByApplicationUserAsync(
            GetOrdersCriteria getOrdersCriteria)
        {
            return _ORM.GetManyWithoutDisable(a =>
                a.ApplicationUserId.Equals(getOrdersCriteria.ApplicationUserId) &&
                a.IsViaRoboTrader == getOrdersCriteria.IsViaRoboTrader &&
                a.IsBackTest == getOrdersCriteria.IsBackTest &&
                (string.IsNullOrWhiteSpace(getOrdersCriteria.SetupTraderId) || a.SetupTraderId == getOrdersCriteria.SetupTraderId) &&
                (!getOrdersCriteria.StartDate.HasValue || !getOrdersCriteria.EndDate.HasValue ||
                a.Date >= getOrdersCriteria.StartDate && a.Date <= getOrdersCriteria.EndDate) &&
                a.OrderType == getOrdersCriteria.OrderType)
               .Include(order => order.BaseAsset)
               .Include(order => order.QuoteAsset)
               .Include(order => order.Exchange)
               .Include(order => order.RelatedOrder).ThenInclude(relatedOrder => relatedOrder.OrderItems)
               .Include(order => order.RelatedOrders).ThenInclude(relatedOrders => relatedOrders.OrderItems)
               .Include(order => order.OrderItems).ThenInclude(orderItems => orderItems.FeeAsset)
               .ToListAsync();
        }
    }
}

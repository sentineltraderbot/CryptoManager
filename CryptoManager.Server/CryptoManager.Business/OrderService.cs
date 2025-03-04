using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoManager.Business
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IExchangeIntegrationStrategyContext _exchangeIntegrationStrategyContext;

        public OrderService(IOrderRepository repository, IExchangeIntegrationStrategyContext exchangeIntegrationStrategyContext)
        {
            _repository = repository;
            _exchangeIntegrationStrategyContext = exchangeIntegrationStrategyContext;
        }

        public Task<Order> CreateOrderAsync(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new InvalidOperationException("Order Item Must be Informed");
            }
            return _repository.InsertAsync(order);
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrdersDetailsByApplicationUserAsync(GetOrdersCriteria getOrdersCriteria)
        {
            var orderTaskList = new List<Task<OrderDetailDTO>>();
            var orders = await _repository.GetAllByApplicationUserAsync(getOrdersCriteria);
            orders.ForEach(order =>
            {
                orderTaskList.Add(BuildOrderDetailAsync(order));
            });
            await Task.WhenAll(orderTaskList.ToArray());
            return orderTaskList.Select(a=> a.Result).OrderBy(a => a.BoughtDate);
        }

        private async Task<OrderDetailDTO> BuildOrderDetailAsync(Order order)
        {
            var paidOrderQuantity = order.OrderItems.Sum(a => a.Quantity);
            var soldOrderQuantity = paidOrderQuantity;
            var orderPrice = CalculateAveragePrice(order.OrderItems.ToList());
            decimal currentPrice = 0;
            bool isCompleted = false;
            DateTime? soldDate = null;
            var currentPriceErrorMessage = string.Empty;
            if (order.RelatedOrders != null && order.RelatedOrders.Any())
            {
                isCompleted = true;
                var relatedOrder = order.RelatedOrders.First();
                currentPrice = CalculateAveragePrice(relatedOrder.OrderItems.ToList());
                soldOrderQuantity = relatedOrder.OrderItems.Sum(a => a.Quantity);
                soldDate = new(relatedOrder.Date.Ticks, DateTimeKind.Utc);
            }
            else
            {
                var priceResult = await _exchangeIntegrationStrategyContext.GetCurrentPriceAsync(
                    order.BaseAsset.Symbol,
                    order.QuoteAsset.Symbol,
                    order.Exchange.ExchangeType);

                if (priceResult.HasSucceded)
                {
                    currentPrice = priceResult.Item.Price;
                }
                else
                {
                    currentPriceErrorMessage = priceResult.ErrorMessage;
                }
            }

            var valuePaidWithFees = orderPrice * paidOrderQuantity;
            var valueSoldWithFees = currentPrice * soldOrderQuantity;
            return new OrderDetailDTO
            {
                Id = order.Id,
                BoughtDate = new(order.Date.Ticks, DateTimeKind.Utc),
                ExchangeName = order.Exchange.Name,
                BaseAssetSymbol = order.BaseAsset.Symbol,
                QuoteAssetSymbol = order.QuoteAsset.Symbol,
                Quantity = paidOrderQuantity,
                AvgPrice = orderPrice,
                ValuePaidWithFees = valuePaidWithFees,
                ValueSoldWithFees = valueSoldWithFees,
                CurrentPrice = currentPrice,
                Profit = valueSoldWithFees - valuePaidWithFees,
                IsCompleted = isCompleted,
                SoldDate = soldDate,
                CurrentPriceErrorMessage = currentPriceErrorMessage
            };
        }

        public static decimal CalculateAveragePrice(List<OrderItem> orderItems)
        {
            var quantity = orderItems.Sum(order => order.Quantity);
            return orderItems.Sum(order => order.Price * order.Quantity) / (quantity == decimal.Zero ? decimal.One : quantity);
        }
    }
}

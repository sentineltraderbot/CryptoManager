using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.Entities;
using CryptoManager.Repository.DatabaseContext;
using CryptoManager.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoManager.Repository.Test.Mocks.Entities
{
    public class MockOrderItem
    {
        public static OrderItemRepository GetDBTestRepository(ApplicationIdentityDbContext context = null)
        {
            var entityORM = MockEntityRepository<OrderItem>.GetRepoTestInMemory(context);
            return new OrderItemRepository(entityORM);
        }

        public static OrderItem GetEntityFake()
        {
            return new OrderItem()
            {
                OrderId = Guid.NewGuid(),
                Price = 1,
                Quantity = 1,
                Fee = 1,
                FeeAssetId = Guid.NewGuid()
            };
        }
    }
}

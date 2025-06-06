using CryptoManager.Domain.Entities;
using CryptoManager.Repository.DatabaseContext;
using CryptoManager.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoManager.Repository.Test.Mocks.Entities
{
    public class MockOrder
    {
        public static OrderRepository GetDBTestRepository(ApplicationIdentityDbContext context = null)
        {
            var entityORM = MockEntityRepository<Order>.GetRepoTestInMemory(context);
            return new OrderRepository(entityORM, MockOrderItem.GetDBTestRepository(context));
        }

        public static Order GetEntityFake()
        {
            return new Order()
            {
                ApplicationUserId = Guid.NewGuid(),
                BaseAssetId = Guid.NewGuid(),
                ExchangeId = Guid.NewGuid(),
                QuoteAssetId = Guid.NewGuid(),
                Date = DateTime.Now
            };
        }
    }
}

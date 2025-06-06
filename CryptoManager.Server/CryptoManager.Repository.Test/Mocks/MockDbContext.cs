using CryptoManager.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoManager.Repository.Test.Mocks
{
    public class MockDbContext
    {
        public static ApplicationIdentityDbContext CreateDBInMemoryContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>();
            builder.UseInMemoryDatabase($"DBTEST{Guid.NewGuid()}")
                 .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return new ApplicationIdentityDbContext(builder.Options);
        }
    }
}

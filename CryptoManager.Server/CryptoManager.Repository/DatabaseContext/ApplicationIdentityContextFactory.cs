using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoManager.Repository.DatabaseContext
{
    class ApplicationIdentityContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityDbContext>
    {
        public ApplicationIdentityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>();
            //builder.UseSqlServer("Server=tcp:sql-sentineltrader-test-eun.database.windows.net,1433;Initial Catalog=sqldb-cryptomanager-test-eun;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';");
            builder.UseSqlite("Filename=.\\CryptoDBLite.sqlite");
            return new ApplicationIdentityDbContext(builder.Options);
        }
    }
}

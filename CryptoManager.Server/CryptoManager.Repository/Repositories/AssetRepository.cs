using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.Entities;
using CryptoManager.Repository.Infrastructure;

namespace CryptoManager.Repository.Repositories;
public class AssetRepository(IORM<Asset> orm) : Repository<Asset>(orm), IAssetRepository
{
}

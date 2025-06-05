using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.Entities;
using CryptoManager.Repository.Infrastructure;

namespace CryptoManager.Repository.Repositories;

public class TokenSaleRepository(IORM<TokenSale> orm) : Repository<TokenSale>(orm), ITokenSaleRepository
{
}

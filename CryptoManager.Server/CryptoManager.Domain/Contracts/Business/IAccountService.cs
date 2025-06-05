using System;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;

namespace CryptoManager.Domain.Contracts.Business;

public interface IAccountService
{
    Task<ApplicationUser> GetUserAsync(Guid userId);
    Task<ObjectResult<ApplicationUser>> UpdateOrCreateUserAsync(ApplicationUser userInfo);
    Task<SimpleObjectResult> UpdateUserAsync(ApplicationUser user);
    Task<SimpleObjectResult> UpdateUserWallet(string walletAddress, Guid userId);
}

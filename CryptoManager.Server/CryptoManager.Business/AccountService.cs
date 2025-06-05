using System;
using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CryptoManager.Business;

public class AccountService : IAccountService
{
    private const string ADMINISTRATOR_ROLE_NAME = "Administrator";
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountService(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
                          IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ObjectResult<ApplicationUser>> UpdateOrCreateUserAsync(ApplicationUser userInfo)
    {
        var user = await _userManager.FindByEmailAsync(userInfo.Email);

        if (user == null)
        {
            var result = await _userManager.CreateAsync(userInfo, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));
            if (!result.Succeeded)
                return ObjectResult<ApplicationUser>.Error("Error creating user");

            result = await AddSuperUserInRoleAdmin(userInfo);
            if (result != null && !result.Succeeded)
                return ObjectResult<ApplicationUser>.Error("Error adding admin");
        }
        else
        {
            user.FacebookId = userInfo.FacebookId;
            user.GoogleId = userInfo.GoogleId;
            user.FirstName = userInfo.FirstName;
            user.LastName = userInfo.LastName;
            user.PictureUrl = userInfo.PictureUrl;
            user.Gender = userInfo.Gender;
            user.Locale = userInfo.Locale;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return ObjectResult<ApplicationUser>.Error("Error updating user");

            result = await AddSuperUserInRoleAdmin(user);
            if (result != null && !result.Succeeded)
                return ObjectResult<ApplicationUser>.Error("Error updating as admin");
        }

        user = await _userManager.FindByEmailAsync(userInfo.Email);

        if (user == null)
        {
            return ObjectResult<ApplicationUser>.Error("login_failure - message:Failed to create local user account.");
        }
        await _signInManager.SignInAsync(user, true, "Bearer");
        return ObjectResult<ApplicationUser>.Success(user);
    }

    public Task<ApplicationUser> GetUserAsync(Guid userId)
    {
        return _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<SimpleObjectResult> UpdateUserWallet(string walletAddress, Guid userId)
    {
        var user = await GetUserAsync(userId);
        user.SolanaWalletAddress = walletAddress;
        return await UpdateUserAsync(user);
    }

    public async Task<SimpleObjectResult> UpdateUserAsync(ApplicationUser user)
    {
        var response = await _userManager.UpdateAsync(user);
        if (!response.Succeeded)
        {
            return SimpleObjectResult.Error("Error updating user");
        }
        return SimpleObjectResult.Success();
    }

    private async Task<IdentityResult> AddSuperUserInRoleAdmin(ApplicationUser user)
    {
        if (user.Email.Equals(_configuration["Authentication:SuperUserEmail"]))
        {
            if (!await _userManager.IsInRoleAsync(user, ADMINISTRATOR_ROLE_NAME))
            {
                return await _userManager.AddToRoleAsync(user, ADMINISTRATOR_ROLE_NAME);
            }
        }
        return null;
    }
}

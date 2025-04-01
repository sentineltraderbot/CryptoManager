using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using CryptoManager.Domain.IntegrationEntities.Facebook;
using CryptoManager.WebApi.Utils;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CryptoManager.WebApi.Controllers
{
    /// <summary>
    /// Responsible to authenticated user and protect all API Controllers
    /// </summary>
    [Produces("application/json")]
    [Route("api/Account")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly JwtFactory _jwtFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly HttpClient _client = new HttpClient();
        private readonly IAccountService _accountService;

        public AccountController(JwtFactory jwtFactory,
                                 SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 IAccountService accountService,
                                 IMapper mapper) : base(mapper)
        {
            _jwtFactory = jwtFactory;
            _userManager = userManager;
            _accountService = accountService;
        }

        /// <summary>
        /// get user logged information
        /// </summary>
        /// <response code="400">If the user not logged</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUserDTO), 200)]
        [ProducesResponseType(typeof(ObjectResult), 400)]
        public async Task<IActionResult> GetUserInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                return new OkObjectResult(_mapper.Map<ApplicationUserDTO>(await _accountService.GetUserAsync(GetUserId())));
            }
            return new ForbidResult();
        }

        /// <summary>
        /// method to authenticate user in facebook and return a jwktoken with your data passing accesstoken from facebook
        /// </summary>
        /// <param name="accessToken">token that identify user in facebook</param>
        /// <returns>JWToken if User is successfully authenticated</returns>
        /// <response code="400">If the user not authenticated in facebook or if occurred other error</response>
        /// <response code="200">If success</response>
        [HttpPost("ExternalLoginFacebook")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ObjectResult), 400)]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public async Task<IActionResult> ExternalLoginFacebook(string accessToken)
        {
            try
            {
                // 1.generate an app access token
                var appAccessTokenResponse = await _client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={WebUtil.FacebookAppId}&client_secret={WebUtil.FacebookAppSecret}&grant_type=client_credentials");
                var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
                // 2. validate the user access token
                var userAccessTokenValidationResponse = await _client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appAccessToken.AccessToken}");
                var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

                if (!userAccessTokenValidation.Data.IsValid)
                {
                    return new BadRequestObjectResult("login_failure - message:Invalid facebook token.");
                }

                // 3. we've got a valid token so we can request user data from fb
                var userInfoResponse = await _client.GetStringAsync($"https://graph.facebook.com/v17.0/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={accessToken}");
                var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

                // 4. ready to create the local user account (if necessary) and jwt
                var userResponse = await _accountService.UpdateOrCreateUserAsync(_mapper.Map<ApplicationUser>(userInfo));
                if (!userResponse.HasSucceded)
                {
                    return new BadRequestObjectResult(userResponse.ErrorMessage);
                }
                return new OkObjectResult(await GenerateToken(userResponse.Item));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("ExternalLoginGoogle")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ObjectResult), 400)]
        [ProducesResponseType(typeof(ObjectResult), 200)]
        public async Task<IActionResult> ExternalLoginGoogle(string token)
        {
            try
            {
                var userInfo = await GoogleJsonWebSignature.ValidateAsync(token);
                var userResponse = await _accountService.UpdateOrCreateUserAsync(_mapper.Map<ApplicationUser>(userInfo));
                if (!userResponse.HasSucceded)
                {
                    return new BadRequestObjectResult(userResponse.ErrorMessage);
                }
                return new OkObjectResult(await GenerateToken(userResponse.Item));

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Name", $"{user.FirstName} {user.LastName}"),
                new Claim("Email", user.Email),
                new Claim("PictureURL", user.PictureUrl),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }

            return _jwtFactory.GenerateToken(claims);
        }
    }
}
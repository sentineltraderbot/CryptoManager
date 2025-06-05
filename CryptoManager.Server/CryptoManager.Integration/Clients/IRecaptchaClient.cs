using System;
using System.Threading.Tasks;
using CryptoManager.Domain.IntegrationEntities.Recaptcha;
using Refit;

namespace CryptoManager.Integration.Clients;

public interface IRecaptchaClient
{
    [Post("/recaptcha/api/siteverify?secret={secretKey}&response={token}")]
    Task<RecaptchaResponse> SiteVerifyAsync(string secretKey, string token);
}

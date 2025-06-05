using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Integration.Clients;
using Microsoft.Extensions.Configuration;

namespace CryptoManager.Business;

public class RecaptchaService : IRecaptchaService
{
    private const string _recaptchaPrivateKeySecretName = "recaptcha-private-key";
    private const double recaptchaScoreLimit = 0.5;
    private readonly IRecaptchaClient _recaptchaClient;
    private readonly IConfiguration _configuration;
    public RecaptchaService(IRecaptchaClient client, IConfiguration configuration)
    {
        _configuration = configuration;
        _recaptchaClient = client;
    }

    public async Task<bool> VerifyTokenAsync(string token)
    {
        var keyVaultUrl = _configuration["AzureKeyVault:Url"];
        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        var secret = await client.GetSecretAsync(_recaptchaPrivateKeySecretName);
        var result = await _recaptchaClient.SiteVerifyAsync(
            secret.Value.Value,
            token);

        return result != null && result.Success && result.Score >= recaptchaScoreLimit;
    }
}
using System.Threading.Tasks;

namespace CryptoManager.Domain.Contracts.Business;

public interface IRecaptchaService
{
    Task<bool> VerifyTokenAsync(string token);
}

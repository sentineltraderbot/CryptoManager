using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoManager.Domain.IntegrationEntities.Recaptcha;

public class RecaptchaResponse
{
    public bool Success { get; set; }
    public float Score { get; set; }
    public string Action { get; set; }
    public DateTime ChallengeTs { get; set; }
    public string Hostname { get; set; }
    public List<string> ErrorCodes { get; set; }
}
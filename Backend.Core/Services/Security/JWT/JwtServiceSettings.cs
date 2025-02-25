namespace Backend.Core.Services.Security.JWT;

public record JwtServiceSettings(string Audience, string Issuer, string SecretKey, int ExpirationInHours);
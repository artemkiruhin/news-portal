namespace Backend.Core.Services.Security.JWT;

public record JwtServiceSettings(string Audience, string Issuer, byte[] SecretKey, int ExpirationInHours);
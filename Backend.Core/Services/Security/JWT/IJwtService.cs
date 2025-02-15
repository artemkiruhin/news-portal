namespace Backend.Core.Services.Security.JWT;

public interface IJwtService
{
    public string GenerateToken(Guid userId, byte[] encodedSecretKey, string audience, string issuer, int expiresInHours);
    public Guid GetUserId(string token);
}
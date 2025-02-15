namespace Backend.Core.Services.Security.JWT;

public interface IJwtService
{
    public string GenerateToken(Guid userId);
    public Guid GetUserId(string token);
}
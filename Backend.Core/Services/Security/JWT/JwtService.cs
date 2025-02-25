using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Services.Security.JWT;

public class JwtService : IJwtService
{
    private readonly string _encodedSecretKey;
    private readonly string _audience;
    private readonly string _issuer;
    private readonly int _expiresInHours;
    
    public JwtService(JwtServiceSettings settings)
    {
        _encodedSecretKey = settings.SecretKey;
        _audience = settings.Audience;
        _issuer = settings.Issuer;
        _expiresInHours = settings.ExpirationInHours;
    }
    
    public string GenerateToken(Guid userId)
    {
        try
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userId.ToString())
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_encodedSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_expiresInHours),
                signingCredentials: creds,
                audience: _audience,
                issuer: _issuer
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при генерации JWT токена", ex);
        }
    }

    public Guid GetUserId(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) 
                ? userId 
                : throw new Exception("Неверный или поддельный токен: ID пользователя не найден");
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при получении ID пользователя из JWT токена", ex);
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Services.Security.JWT;

public class JwtService : IJwtService
{
    public string GenerateToken(Guid userId, byte[] encodedSecretKey, string audience, string issuer, int expiresInHours)
    {
        try
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userId.ToString())
            };
            
            var key = new SymmetricSecurityKey(encodedSecretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiresInHours),
                signingCredentials: creds,
                audience: audience,
                issuer: issuer
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
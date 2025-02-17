using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Services.Security.Hash;
using Backend.Core.Services.Security.JWT;

namespace Backend.Core.UseCases;

public class AuthorizeUserUseCase
{
    private readonly IUnitOfWork _database;
    private readonly IHasher _hasher;
    private readonly IJwtService _jwtService;

    public AuthorizeUserUseCase(IUnitOfWork database, IHasher hasher, IJwtService jwtService)
    {
        _database = database;
        _hasher = hasher;
        _jwtService = jwtService;
    }
    
    public async Task<Result<string>> ExecuteAsync(string username, string password)
    {
        try
        {
            var hashedPassword = _hasher.Hash(password);
            var user = await _database.UserRepository.GetByUsernameAndPasswordHashAsync(username, hashedPassword);

            if (user == null) return Result<string>.Failure("Неверный логин или пароль");
            
            var jwtToken = _jwtService.GenerateToken(user.Id);
            return Result<string>.Success(jwtToken);
        }
        catch (Exception e)
        {
            return Result<string>.Failure("Ошибка аторизации" + e.Message);
        }
    }
}
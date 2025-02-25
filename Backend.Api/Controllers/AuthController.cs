using Backend.Core.Extensions.Utils;
using Backend.Core.UseCases;
using Backend.Core.UseCases.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthorizeUserUseCase _authorizeUserUseCase;
        private readonly RegisterUserUseCase _registerUserUseCase;
        private readonly DbSeeder _dbSeeder;

        public AuthController(AuthorizeUserUseCase authorizeUserUseCase, RegisterUserUseCase registerUserUseCase, DbSeeder dbSeeder)
        {
            _authorizeUserUseCase = authorizeUserUseCase;
            _registerUserUseCase = registerUserUseCase;
            _dbSeeder = dbSeeder;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _authorizeUserUseCase.ExecuteAsync(request, ct);
                if (!result.IsSuccess) return Unauthorized(result.ErrorMessage);
                
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = false,   
                    Expires = DateTime.UtcNow.AddDays(3) 
                };

                Response.Cookies.Append("jwt", result.Value, cookieOptions);

                return Ok(new { Token = result.Value });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _registerUserUseCase.ExecuteAsync(request, ct);
                if (result.IsSuccess) return Ok(result.Value);
                return Unauthorized(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None
            });
            return Ok(new { Message = "Выход из аккаунта совершен" });
        }

        [AllowAnonymous]
        [HttpPost("seed")]
        public IActionResult Seed()
        {
            _dbSeeder.Seed();
            return Ok();
        }
    }
}

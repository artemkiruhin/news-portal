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

        public AuthController(AuthorizeUserUseCase authorizeUserUseCase, RegisterUserUseCase registerUserUseCase)
        {
            _authorizeUserUseCase = authorizeUserUseCase;
            _registerUserUseCase = registerUserUseCase;
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
                    SameSite = SameSiteMode.None, 
                    Expires = DateTime.UtcNow.AddDays(3) 
                };

                Response.Cookies.Append("jwt", result.Value, cookieOptions);

                return Ok(new { Message = "Авторизация успешна" });
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
    }
}

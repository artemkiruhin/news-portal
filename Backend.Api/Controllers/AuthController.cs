using Backend.Core.UseCases;
using Backend.Core.UseCases.Contracts;
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
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _authorizeUserUseCase.ExecuteAsync(request, ct);
                if (result.IsSuccess) return Ok(result.Value);
                return Unauthorized(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Login([FromBody] RegisterSettings request, CancellationToken ct)
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
    }
}

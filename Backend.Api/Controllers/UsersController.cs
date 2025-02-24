using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.UseCases;
using Backend.Core.UseCases.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _database;
        private readonly BlockAccountUseCase _blockAccountUseCase;
        private readonly UpdateEmployeeInfoUseCase _updateEmployeeInfoUseCase;

        public UsersController(IUnitOfWork unitOfWork, BlockAccountUseCase blockAccountUseCase, UpdateEmployeeInfoUseCase updateEmployeeInfoUseCase)
        {
            _database = unitOfWork;
            _blockAccountUseCase = blockAccountUseCase;
            _updateEmployeeInfoUseCase = updateEmployeeInfoUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UserFilterRequest request, CancellationToken ct)
        {
            try
            {
                var users = await _database.UserRepository.GetFilteredAsync(user =>
                        (!string.IsNullOrEmpty(request.Email) && user.Email.Contains(request.Email)) ||
                        (request.HasPublishRights.HasValue && user.HasPublishRights == request.HasPublishRights.Value)
                    , ct);

                var response = users.Select(user => new UserResponse(
                    user.Id,
                    user.Username,
                    user.Email ?? "",
                    user.HasPublishRights,
                    user.RegisteredAt,
                    new DepartmentShortResponse(
                        user.DepartmentId,
                        user.Department.Name
                    ),
                    new UserStatisticsResponse(
                        user.Posts.Count,
                        user.Comments.Count,
                        user.Reactions.Count
                    )
                ));

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id, CancellationToken ct)
        {
            try
            {
                var user = await _database.UserRepository.GetByIdAsync(id, ct);
                if (user == null) return NotFound();

                var response = new UserResponse(
                    user.Id,
                    user.Username,
                    user.Email ?? "",
                    user.HasPublishRights,
                    user.RegisteredAt,
                    new DepartmentShortResponse(
                        user.DepartmentId,
                        user.Department.Name
                    ),
                    new UserStatisticsResponse(
                        user.Posts.Count,
                        user.Comments.Count,
                        user.Reactions.Count
                    )
                );

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserResponse>> GetByUsername(string username, CancellationToken ct)
        {
            try
            {
                var user = await _database.UserRepository.GetByUsernameAsync(username, ct);
                if (user == null) return NotFound();

                var response = new UserResponse(
                    user.Id,
                    user.Username,
                    user.Email ?? "",
                    user.HasPublishRights,
                    user.RegisteredAt,
                    new DepartmentShortResponse(
                        user.DepartmentId,
                        user.Department.Name
                    ),
                    new UserStatisticsResponse(
                        user.Posts.Count,
                        user.Comments.Count,
                        user.Reactions.Count
                    )
                );

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("block/{id:guid}")]
        public async Task<ActionResult> BlockUser(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await _blockAccountUseCase.ExecuteAsync(id, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("edit")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _updateEmployeeInfoUseCase.ExecuteAsync(request, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
using Backend.Core.Models.Entities;
using Backend.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactionsController : ControllerBase
    {
        private readonly ReactionStatsByPostUseCase _reactionStatsByPostUseCase;
        private readonly ReactPostUseCase _reactionPostUseCase;
        private readonly IUnitOfWork _database;

        public ReactionsController(ReactionStatsByPostUseCase reactionStatsByPostUseCase,
            ReactPostUseCase reactionPostUseCase, IUnitOfWork database)
        {
            _reactionStatsByPostUseCase = reactionStatsByPostUseCase;
            _reactionPostUseCase = reactionPostUseCase;
            _database = database;
        }

        [HttpGet("post/{postId:guid}")]
        public async Task<IActionResult> GetAllByPostId(Guid postId, CancellationToken ct)
        {
            try
            {
                var result = await _reactionStatsByPostUseCase.ExecuteAsync(postId, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ReactionCreateRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _reactionPostUseCase.ExecuteAsync(request, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                var react = await _database.ReactionRepository.GetByIdAsync(id, ct);
                if (react == null) return NotFound();
                var result = await _database.ReactionRepository.DeleteAsync(react, ct);
                return Ok(result.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

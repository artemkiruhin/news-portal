using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.UseCases;
using Backend.Core.UseCases.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork _database;
        private readonly CreatePostUseCase _createPostUseCase;
        private readonly DeletePostUseCase _deletePostUseCase;
        private readonly UpdatePostUseCase _updatePostUseCase;
        private readonly FilterPostsUseCase _filterPostsUseCase;

        public PostsController(FilterPostsUseCase filterPostsUseCase, IUnitOfWork unitOfWork,
            CreatePostUseCase createPostUseCase, DeletePostUseCase deletePostUseCase,
            UpdatePostUseCase updatePostUseCase)
        {
            _filterPostsUseCase = filterPostsUseCase;
            _database = unitOfWork;
            _createPostUseCase = createPostUseCase;
            _deletePostUseCase = deletePostUseCase;
            _updatePostUseCase = updatePostUseCase;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllPosts(string? fullContent, Guid? publisherId, List<Guid>? departmentIds,
            DateTime? startDate, DateTime? endDate, CancellationToken ct)
        {
            try
            {
                var result = await _filterPostsUseCase.ExecuteAsync(fullContent, publisherId, departmentIds, startDate, endDate, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("id/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await _database.PostRepository.GetByIdAsync(id, ct);
                if (result == null) return NotFound();
                var dto = new PostResponse
                (
                    result.Id,
                    result.Title,
                    result.Subtitle,
                    result.Content,
                    result.Publisher.Username,
                    result.PublishedAt,
                    result.LastModifiedAt,
                    new ReactionStatsResponse(
                        result.Reactions.Count(x => x.Type == ReactionType.Like),
                        result.Reactions.Count(x => x.Type == ReactionType.Dislike),
                        result.Reactions.Count(x => x.Type == ReactionType.Checked)
                    )
                );
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost(CreatePostSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _createPostUseCase.ExecuteAsync(request, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await _deletePostUseCase.ExecuteAsync(id, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("edit")]
        public async Task<IActionResult> EditPost(UpdatePostSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _updatePostUseCase.ExecuteAsync(request, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}

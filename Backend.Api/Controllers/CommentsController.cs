using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.UseCases;
using Backend.Core.UseCases.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _database;
        private readonly CommentPostUseCase _commentPostUseCase;
        private readonly DeleteCommentUseCase _deleteCommentUseCase;
        private readonly UpdateCommentUseCase _updateCommentUseCase;
        private readonly ReplyCommentUseCase _replyCommentUseCase;

        public CommentsController(IUnitOfWork database, CommentPostUseCase commentPostUseCase,
            DeleteCommentUseCase deleteCommentUseCase, UpdateCommentUseCase updateCommentUseCase, ReplyCommentUseCase replyCommentUseCase)
        {
            _database = database;
            _commentPostUseCase = commentPostUseCase;
            _deleteCommentUseCase = deleteCommentUseCase;
            _updateCommentUseCase = updateCommentUseCase;
            _replyCommentUseCase = replyCommentUseCase;
        }

        [HttpGet("post/{id:guid}")]
        public async Task<IActionResult> GetAllByPostId(Guid id, CancellationToken ct)
        {
            try
            {
                var comments = await _database.CommentRepository.GetByPostIdAsync(id, ct);
                var dtos = comments.Select(comment => new CommentResponse(
                        comment.Id,
                        comment.Content,
                        new AuthorShortResponse(
                            comment.SenderId,
                            comment.Sender.Username
                        ),
                        comment.CreatedAt,
                        comment.UpdatedAt,
                        comment.ReplyId.HasValue
                            ? new CommentReplyResponse(
                                comment.ReplyId.Value,
                                comment.Reply.Content,
                                new AuthorShortResponse(
                                    comment.Reply.SenderId,
                                    comment.Reply.Sender.Username
                                )
                            )
                            : null
                    )
                );
                return Ok(dtos);
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
                var comment = await _database.CommentRepository.GetByIdAsync(id, ct);
                if (comment == null) return NotFound();
                var dto = new CommentResponse(
                    comment.Id,
                    comment.Content,
                    new AuthorShortResponse(
                        comment.SenderId,
                        comment.Sender.Username
                    ),
                    comment.CreatedAt,
                    comment.UpdatedAt,
                    comment.ReplyId.HasValue
                        ? new CommentReplyResponse(
                            comment.ReplyId.Value,
                            comment.Reply.Content,
                            new AuthorShortResponse(
                                comment.Reply.SenderId,
                                comment.Reply.Sender.Username
                            )
                        )
                        : null
                );
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCommentSettings request, CancellationToken ct)
        {
            try
            {
                var result = await _commentPostUseCase.ExecuteAsync(request, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Create(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await _deleteCommentUseCase.ExecuteAsync(id, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("edit")]
        public async Task<IActionResult> Create(Guid commentId, string content, CancellationToken ct)
        {
            try
            {
                var result = await _updateCommentUseCase.ExecuteAsync(commentId, content, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("reply")]
        public async Task<IActionResult> Reply(string content, Guid postId, Guid senderId, Guid replyId, CancellationToken ct)
        {
            try
            {
                var result = await _replyCommentUseCase.ExecuteAsync(content, postId, senderId, replyId, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

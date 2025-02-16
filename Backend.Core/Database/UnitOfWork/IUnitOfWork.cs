using Backend.Core.Database.Repositories.Interfaces;

namespace Backend.Core.Database.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    IUserRepository UserRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    ILogRepository LogRepository { get; }
    ICommentRepository CommentRepository { get; }
    IPostRepository PostRepository { get; }
    IReactionRepository ReactionRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
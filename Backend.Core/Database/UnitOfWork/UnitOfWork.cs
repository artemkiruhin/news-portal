using System.Data;
using Backend.Core.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Core.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction? _transaction;

        public IUserRepository UserRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public ILogRepository LogRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IPostRepository PostRepository { get; }
        public IReactionRepository ReactionRepository { get; }

        public UnitOfWork(DbContext context, 
                          IUserRepository userRepository,
                          IDepartmentRepository departmentRepository,
                          ILogRepository logRepository,
                          ICommentRepository commentRepository,
                          IPostRepository postRepository,
                          IReactionRepository reactionRepository)
        {
            _context = context;
            UserRepository = userRepository;
            DepartmentRepository = departmentRepository;
            LogRepository = logRepository;
            CommentRepository = commentRepository;
            PostRepository = postRepository;
            ReactionRepository = reactionRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => 
            await _context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                throw new InvalidOperationException("Транзакция уже была начата.");

            _transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Нет начатой транзакции.");

            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Нет начатой транзакции.");

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            await _context.DisposeAsync();
        }
    }
}

using Backend.Core.Database;
using Backend.Core.Models.Entities;
using Backend.Core.Services.Security.Hash;

namespace Backend.Core.Extensions.Utils
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;
        private readonly IHasher _hasher;

        public DbSeeder(AppDbContext context, IHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public void Seed()
        {
            // Очистка базы данных перед заполнением
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Генерация отделов
            var departments = GenerateDepartments(10);
            _context.Departments.AddRange(departments);
            _context.SaveChanges();

            // Генерация пользователей
            var users = GenerateUsers(50, departments);
            _context.Users.AddRange(users);
            _context.SaveChanges();

            // Генерация постов
            var posts = GeneratePosts(100, users);
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Генерация комментариев
            var comments = GenerateComments(500, posts, users);
            _context.Comments.AddRange(comments);
            _context.SaveChanges();

            // Генерация реакций
            var reactions = GenerateReactions(1000, posts, users);
            _context.Reactions.AddRange(reactions);
            _context.SaveChanges();

            // Генерация логов
            var logs = GenerateLogs(200);
            _context.Logs.AddRange(logs);
            _context.SaveChanges();
        }

        private List<DepartmentEntity> GenerateDepartments(int count)
        {
            var departments = new List<DepartmentEntity>();
            for (int i = 1; i <= count; i++)
            {
                departments.Add(DepartmentEntity.Create($"Отдел {i}"));
            }
            return departments;
        }

        private List<UserEntity> GenerateUsers(int count, List<DepartmentEntity> departments)
        {
            var users = new List<UserEntity>();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                var department = departments[random.Next(departments.Count)];
                users.Add(UserEntity.Create(
                    $"Пользователь {i}",
                    $"hashed_password_{i}",
                    $"user{i}@example.com",
                    random.Next(2) == 1, // Случайные права на публикацию
                    department.Id
                ));
            }
            users.Add(UserEntity.Create(
                $"user",
                _hasher.Hash("1"),
                $"user@example.com",
                false, 
                departments[0].Id
            ));
            users.Add(UserEntity.Create(
                $"admin",
                _hasher.Hash("1"),
                $"admin@example.com",
                true, 
                departments[0].Id
            ));
            return users;
        }

        private List<PostEntity> GeneratePosts(int count, List<UserEntity> users)
        {
            var posts = new List<PostEntity>();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                var user = users[random.Next(users.Count)];
                posts.Add(PostEntity.Create(
                    $"Пост {i}",
                    $"Содержание поста {i}",
                    user.Id,
                    random.Next(2) == 1 ? $"Подзаголовок поста {i}" : null
                ));
            }
            return posts;
        }

        private List<CommentEntity> GenerateComments(int count, List<PostEntity> posts, List<UserEntity> users)
        {
            var comments = new List<CommentEntity>();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                var post = posts[random.Next(posts.Count)];
                var user = users[random.Next(users.Count)];
                var replyId = comments.Any() && random.Next(2) == 1 ? comments[random.Next(comments.Count)].Id : (Guid?)null;
                comments.Add(CommentEntity.Create(
                    $"Комментарий {i}",
                    post.Id,
                    user.Id,
                    replyId
                ));
            }
            return comments;
        }

        private List<ReactionEntity> GenerateReactions(int count, List<PostEntity> posts, List<UserEntity> users)
        {
            var reactions = new List<ReactionEntity>();
            var random = new Random();
            var reactionTypes = Enum.GetValues(typeof(ReactionType)).Cast<ReactionType>().ToList();
            for (int i = 1; i <= count; i++)
            {
                var post = posts[random.Next(posts.Count)];
                var user = users[random.Next(users.Count)];
                var reactionType = reactionTypes[random.Next(reactionTypes.Count)];
                reactions.Add(ReactionEntity.Create(
                    reactionType,
                    post.Id,
                    user.Id
                ));
            }
            return reactions;
        }

        private List<LogEntity> GenerateLogs(int count)
        {
            var logs = new List<LogEntity>();
            var random = new Random();
            var logTypes = Enum.GetValues(typeof(LogType)).Cast<LogType>().ToList();
            for (int i = 1; i <= count; i++)
            {
                var logType = logTypes[random.Next(logTypes.Count)];
                logs.Add(LogEntity.Create(
                    $"Лог {i}",
                    logType
                ));
            }
            return logs;
        }
    }
}
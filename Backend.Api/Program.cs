using System.Runtime.Intrinsics.Arm;
using System.Text;
using Backend.Core.Database;
using Backend.Core.Database.Repositories.Implementations;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Database.UnitOfWork;
using Backend.Core.Services.Security.Hash;
using Backend.Core.Services.Security.JWT;
using Backend.Core.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(configuration.GetConnectionString("Database")).UseLazyLoadingProxies()
);

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IHasher, SHA256Hasher>();
builder.Services.AddScoped<IJwtService, JwtService>(provider => new JwtService(new JwtServiceSettings(
    Audience: configuration["JWT:Audience"] ?? throw new ArgumentException("Missing JWT:Audience"),
    Issuer: configuration["JWT:Issuer"] ?? throw new ArgumentException("Missing JWT:Issuer"),
    SecretKey: Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? throw new ArgumentException("Missing JWT:Key")),
    ExpirationInHours: int.Parse(configuration["JWT:Expires"] ?? throw new ArgumentException("Missing JWT:Expires"))
)));

builder.Services.AddScoped<AuthorizeUserUseCase>();
builder.Services.AddScoped<BlockAccountUseCase>();
builder.Services.AddScoped<CommentPostUseCase>();
builder.Services.AddScoped<CreateDepartmentUseCase>();
builder.Services.AddScoped<CreatePostUseCase>();
builder.Services.AddScoped<DeleteCommentUseCase>();
builder.Services.AddScoped<DeletePostUseCase>();
builder.Services.AddScoped<FilterPostsUseCase>();
builder.Services.AddScoped<ReactionStatsByPostUseCase>();
builder.Services.AddScoped<ReactPostUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<ReplyCommentUseCase>();
builder.Services.AddScoped<ResetPasswordInfoUseCase>();
builder.Services.AddScoped<UpdateCommentUseCase>();
builder.Services.AddScoped<UpdateDepartmentUseCase>();
builder.Services.AddScoped<UpdateEmployeeInfoUseCase>();
builder.Services.AddScoped<UpdatePostUseCase>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
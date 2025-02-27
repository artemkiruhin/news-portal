using System.Runtime.Intrinsics.Arm;
using System.Text;
using Backend.Core.Database;
using Backend.Core.Database.Repositories.Implementations;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Database.UnitOfWork;
using Backend.Core.Extensions.Utils;
using Backend.Core.Services.Security.Hash;
using Backend.Core.Services.Security.JWT;
using Backend.Core.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true, 
            ValidateAudience = true, 
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true, 
            ValidIssuer = configuration["JWT:Issuer"], 
            ValidAudience = configuration["JWT:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine(configuration["JWT:Issuer"]);
                Console.WriteLine(configuration["JWT:Audience"]);
                Console.WriteLine(configuration["JWT:Key"]);
                
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
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
builder.Services.AddScoped<IJwtService>(provider => new JwtService(new JwtServiceSettings(
    Audience: configuration["JWT:Audience"] ?? throw new ArgumentException("Missing JWT:Audience"),
    Issuer: configuration["JWT:Issuer"] ?? throw new ArgumentException("Missing JWT:Issuer"),
    SecretKey: configuration["JWT:Key"] ?? throw new ArgumentException("Missing JWT:Key"),
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

builder.Services.AddScoped<DbSeeder>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

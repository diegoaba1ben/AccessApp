using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Infrastructure.Queries.Interfaces;
using AccessAppUser.Infrastructure.Queries.Implementations;
using AccessAppUser.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();

// Obtener configuración con valores predeterminados en caso de error
var dbHost = DotNetEnv.Env.GetString("DB_HOST", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost");
var dbPort = DotNetEnv.Env.GetString("DB_PORT", "3306");
var dbUser = DotNetEnv.Env.GetString("DB_USER", "root");
var dbPassword = DotNetEnv.Env.GetString("DB_PASSWORD", "");
var dbName = DotNetEnv.Env.GetString("DB_NAME", "AccessAppDb");

var connectionString = $"Server={dbHost};Port={dbPort};User={dbUser};Password={dbPassword};Database={dbName};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 405))));

// Configuración de Redis en la inyección de dependencias
var redisConnection = DotNetEnv.Env.GetString("REDIS_CONNECTION", 
        Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379");
// Configuración de Redis con IConnectionMultiplexer
builder.Services.AddSingleton<IConnectionMultiplexer>(provider => 
        ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de AutoMapper
builder.Services.AddApplicationMappers();

// Registro de repositorio genérico
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Registro de repositorios especializados
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IGesPassRepository, GesPassRepository>();

// Resgistro de consultas
builder.Services.AddScoped<IProfileQueries, ProfileQueries>();
builder.Services.AddScoped<IUserQueries, UserQueries>();

var app = builder.Build();

// Aplicar CORS
app.UseCors("AllowAll");

// Verificar la existencia de la base de datos
EnsureDatabaseCreated(app);

void EnsureDatabaseCreated(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            if (context.Database.EnsureCreated())
            {
                logger.LogInformation("Base de datos creada exitosamente.");
            }
            else
            {
                logger.LogInformation("La base de datos ya existe.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al verificar la base de datos.");
            throw;
        }
    }
}

// Configuración de middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
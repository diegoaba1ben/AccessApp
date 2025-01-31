using AccessAppUser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DotNetEnv; // Para cargar variables de entorno
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();

// Construcción de la cadena de conexión usando variables de entorno
var connectionString = $"Server={DotNetEnv.Env.GetString("DB_HOST")};" +
                       $"Port={DotNetEnv.Env.GetString("DB_PORT")};" +
                       $"User={DotNetEnv.Env.GetString("DB_USER")};" +
                       $"Password={DotNetEnv.Env.GetString("DB_PASSWORD")};" +
                       $"Database={DotNetEnv.Env.GetString("DB_NAME")};";

// Configurar el DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 405)) // Versión compatible con MySQL
    );
});

// Habilitar CORS para el frontend
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

var app = builder.Build();

// Aplicar middleware de CORS
app.UseCors("AllowAll");

// Verifica la existencia de la base de datos
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
                logger.LogInformation(" Base de datos creada exitosamente.");
            }
            else
            {
                logger.LogInformation(" La base de datos ya existe.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, " Error al verificar la base de datos.");
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



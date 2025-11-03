using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Services ----------------
builder.Services.AddControllers()
    // Mostrar enums como cadenas en JSON (p.ej. "CreditCard")
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Selección de base de datos por variable de entorno DBConnection2Use
string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");

switch (connection2Database)
{
    case "SQLite":
        // BD en memoria (útil para pruebas)
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        // Persistente alternativa:
        // DbConnection _connection = new SqliteConnection("Data Source=Application.db;Cache=Shared");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));
        break;

    default:
        // LocalDB por defecto (connection string en appsettings.json)
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}

// Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// (Opcional) Relajar política de contraseñas para pruebas
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "AppForSEII2526.API",
            Version = "v1",
            Description = "API for purchasing and renting devices",
            License = new OpenApiLicense { Name = "MIT License", Url = new Uri("https://opensource.org/license/mit/") },
            Contact = new OpenApiContact { Name = "Software Engineering II Team", Email = "isii@on.uclm.es" },
        });

    // Mantener nombres reales de acciones en Swagger
    options.CustomOperationIds(apiDescription =>
        apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
});

var app = builder.Build();

// Mapear endpoints de Identity (solo si quieres exponer /register, /login, etc.)
// app.MapIdentityApi<ApplicationUser>();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// ---------------- DB Migrate + Seed ----------------
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Crear o migrar BD
        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();
        else
            db.Database.Migrate();

        // Seed (roles, usuarios, modelos/dispositivos y compra opcional)
        SeedData.Initialize(db, scope.ServiceProvider, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred initializing/seeding the DB.");
    }
}

// ---------------- Pipeline HTTP ----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.DisplayOperationId());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Expose Program for integration tests
public partial class Program { }

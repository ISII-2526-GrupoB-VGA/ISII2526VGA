using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
// usando tu provider de logging (déjalo comentado si no tienes RabbitMQ levantado)
    

var builder = WebApplication.CreateBuilder(args);

// ----------------- Controllers + JSON -----------------
builder.Services.AddControllers()
    // mostrar enums como cadenas
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ----------------- Base de datos -----------------
string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");

// Igual que en AppForMovies: SQLite / AzureSQL / LocalDB
switch (connection2Database)
{
    case "SQLite":
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        // Si prefieres una BD persistente, usa esta otra línea:
        // DbConnection _connection = new SqliteConnection("Data Source=Application.db;Cache=Shared");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));
        break;

    default:
        // LocalDB / SQL Server definido en appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}

// ----------------- Identity -----------------
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// (Opcional) relajar contraseñas para pruebas
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
});

// ----------------- Swagger -----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "AppForSEII2526.API",
            Version = "v1",
            Description = "API for renting and purchasing devices",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/license/mit/")
            },
            Contact = new OpenApiContact
            {
                Name = "Software Engineering II Team",
                Email = "isii@on.uclm.es"
            },
        });

    // usar el nombre real de cada acción como OperationId
    options.CustomOperationIds(apiDescription =>
        apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
            ? methodInfo.Name
            : null);
});

// ----------------- Logger RabbitMQ (opcional) -----------------
// Si NO quieres depender de Docker/RabbitMQ, déjalo comentado.
// Cuando quieras activarlo, descomenta la línea siguiente y configura appsettings.json.
// builder.Logging.AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();

// ----------------- DB init + Seed -----------------
var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();
        else
            db.Database.Migrate();

        // Rellenar datos iniciales (roles, usuarios, dispositivos, etc.)
        SeedData.Initialize(db, scope.ServiceProvider, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// ----------------- Pipeline HTTP -----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Exponer Program para los tests de integración
public partial class Program { }

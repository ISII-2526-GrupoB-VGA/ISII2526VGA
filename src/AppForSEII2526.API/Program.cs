using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;

    

var builder = WebApplication.CreateBuilder(args);

//Controllers + JSON
builder.Services.AddControllers()
    // mostrar enums como cadenas
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Selección de base de datos por variable de entorno DBConnection2Use
string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use"); //Esto creo que se debe quedar así


switch (connection2Database)
{
    case "SQLite":
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));
        break;

    default:
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}

//Identity
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//relajar contraseñas para pruebas
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
});

//Swagger
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

    options.CustomOperationIds(apiDescription =>
        apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
            ? methodInfo.Name
            : null);
});

var app = builder.Build();

//DB init + Seed
var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();    //Esto se queda tal cual. Creo
        else
            db.Database.Migrate();          //Esto se queda tal cual. Creo 

        // Rellenar datos iniciales
        SeedData.Initialize(db, scope.ServiceProvider, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

//Pipeline HTTP
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

public partial class Program { }

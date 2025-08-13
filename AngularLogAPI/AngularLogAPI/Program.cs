using AngularLogAPI.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Configuración JWT
builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetValue<string>("settings:secretkey");
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// 🌐 Configuración CORS
var corsPolicyName = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, policy =>
    {
        policy.WithOrigins(
                "http://localhost:8080", 
                "https://frontend-179567002647.us-central1.run.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 📦 Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnStr")));

// 🌐 Puerto para Render
builder.WebHost.UseUrls("http://*:80");

// 🏗️ Construcción de la app
var app = builder.Build();

// 🚀 Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsPolicyName);
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ✅ Ruta de prueba para Render
app.MapGet("/", () => "API Cripto está corriendo 🚀");

app.Run();

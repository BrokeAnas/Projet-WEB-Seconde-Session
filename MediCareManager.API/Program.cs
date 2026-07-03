using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediCareManager.API.EndPoints;
using MediCareManager.API.Middleware;
using MediCareManager.Core;
using MediCareManager.Core.Settings;
using MediCareManager.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ----- Connexion DB (transmise aux repositories Dapper via l'Infrastructure) -----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// ----- Paramètres JWT (lus depuis appsettings, injectés dans Core) -----
var jwtSettings = new JwtSettings
{
    Key = builder.Configuration["Jwt:Key"]!,
    Issuer = builder.Configuration["Jwt:Issuer"]!,
    Audience = builder.Configuration["Jwt:Audience"]!,
    ExpiryHours = int.TryParse(builder.Configuration["Jwt:ExpiryHours"], out var h) ? h : 8
};
builder.Services.AddSingleton(jwtSettings);

// ----- DI : UseCases (Core) + Gateways/Repositories/Sécurité (Infrastructure) -----
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(connectionString);

#region Authentication and Authorization

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // On conserve les noms de claims tels quels ("sub").
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = "sub",
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

#endregion

#region Cors

// ----- CORS pour Angular (localhost:4200) -----
builder.Services.AddCors(options => options.AddPolicy("Angular",
    p => p.WithOrigins("http://localhost:4200")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()));

#endregion

// ----- Sérialisation JSON snake_case des Minimal API (alignée sur les modèles Angular) -----
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    // Permet aux propriétés numériques (long) d'être lues depuis une chaîne JSON.
    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MediCare Manager API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Saisir le token JWT (sans le préfixe « Bearer »).",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    options.AddSecurityDefinition("Bearer", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
});

#endregion

var app = builder.Build();

// ----- Middleware -----
app.UseSwagger();
app.UseSwaggerUI();
// Traduction centralisée des exceptions métier en codes HTTP.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();

#region Endpoints

app.AddAuthEndPoints();
app.AddPatientEndPoints();
app.AddMedecinEndPoints();
app.AddSucursaleEndPoints();
app.AddRendezVousEndPoints();

#endregion

app.Run();

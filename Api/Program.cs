global using Api.ExpositionModels;
global using Api.Models;
global using Api.Models.Entities;
global using Api.Models.Enums;
global using Api.Utilitaries;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using static Api.Utilitaries.AuthorizeRolesAttribute;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


bool dev = true;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Singletons
try
{
    builder.Services.AddSingleton(new DataValidator());
    builder.Services.AddSingleton(new Mapper());
    builder.Services.AddSingleton(new LevenshteinDistance());
    builder.Services.AddSingleton(new ExceptionHandler(new DirectoryInfo(Directory.GetCurrentDirectory()) + "/exceptions.json"));
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    return;
}

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Swagger configs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme jwtSecurityScheme = new OpenApiSecurityScheme()
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Bearer :",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        },
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token:Key").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddDbContext<ModelsContext>(options =>
{
    if (dev)
    {
        options.UseNpgsql(
        builder.Configuration.GetConnectionString("MusicOverflowHeroku"),
        optionBuilder => optionBuilder.MigrationsAssembly("Api"));
    }
    else
    {
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("MusicOverflowAzure"),
        optionBuilder => optionBuilder.MigrationsAssembly("Api"));
    }
});

builder.Services.AddCors();

WebApplication app = builder.Build();

if (dev)
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
}

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

// Swagger configs
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

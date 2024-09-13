using Abdt.Loyal.NoteSaver.BusinessLogic;
using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.BusinessLogic.Validation;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Options;
using Abdt.Loyal.NoteSaver.Repository;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

var dbConnection = builder.Configuration.GetConnectionString("PostgresConnection");
if (string.IsNullOrWhiteSpace(dbConnection))
    throw new ArgumentException(nameof(dbConnection));

builder.Services.AddDbContext<NoteContext>(options => options.UseNpgsql(dbConnection));

var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
var redisCacheSettings = builder.Configuration.GetSection("RedisCacheSettings");
if (string.IsNullOrWhiteSpace(redisConnectionString))
    throw new ArgumentException(nameof(redisConnectionString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = redisCacheSettings.GetValue<string>("InstanceName");
});

builder.Services.AddScoped<IStorageLogic<Note>, StorageLogic>();
builder.Services.AddScoped<IValidator<Note>, Validator>();
builder.Services.AddScoped<ICache, CachingItem>();
builder.Services.AddScoped<NoteDbRepository>();
builder.Services.AddScoped<IRepository<Note>, CachedNoteRepository>();

builder.Services.AddOptions<LogicArgs>()
    .BindConfiguration("Flags")
    .ValidateDataAnnotations();


var key = Encoding.ASCII.GetBytes("MySuperSecretKeyThatVeryHardToGuess");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "¬ведите токен в формате Bearer {токен}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

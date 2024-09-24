using Abdt.Loyal.NoteSaver.BusinessLogic;
using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.BusinessLogic.Validation;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Options;
using Abdt.Loyal.NoteSaver.Repository;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("aaa",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("aaa");


app.UseHttpsRedirection();
app.MapControllers();

app.Run();

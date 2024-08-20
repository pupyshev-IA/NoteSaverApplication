using Abdt.Loyal.NoteSaver.BusinessLogic;
using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.BusinessLogic.Validation;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Options;
using Abdt.Loyal.NoteSaver.DTO;
using Abdt.Loyal.NoteSaver.Repository;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

var dbConnection = builder.Configuration.GetConnectionString("DbConnection");
if (string.IsNullOrWhiteSpace(dbConnection))
    throw new ArgumentException(nameof(dbConnection));

builder.Services.AddDbContext<NoteContext>(options => options.UseNpgsql(dbConnection));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IStorageLogic<Note>, StorageLogic>();
builder.Services.AddScoped<IValidator<Note>, Validator>();
builder.Services.AddScoped<NoteDbRepository>();
builder.Services.AddScoped<IRepository<Note>, CachedNoteRepository>();

builder.Services.AddOptions<LogicArgs>()
    .BindConfiguration("Flags")
    .ValidateDataAnnotations();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/v1/notes/{id}", async (long id, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var isValidId = id >= 1;

    if (!isValidId)
        return Results.NotFound();

    var note = await logic.Get(id);
    var adaptedNote = note.Adapt<NoteDtoOut>();

    return Results.Ok(adaptedNote);
}).Produces<NoteDtoOut>()
  .Produces(404);

app.MapPost("api/v1/notes/add", async (NoteDto noteDto, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var note = new Note() { Title = noteDto.Title, Content = noteDto.Content };

    if (!noteValidator.Validate(note).IsValid)
        return Results.UnprocessableEntity();

    var addedNoteId = await logic.Add(note);

    return Results.Created(new Uri("api/v1/notes/", UriKind.Relative), addedNoteId);
}).Produces(201)
  .Produces(422);

app.MapPut("api/v1/notes/update", async (NoteDtoUpdate noteDtoUpdate, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var note = new Note() { Id = noteDtoUpdate.Id, Title = noteDtoUpdate.Title, Content = noteDtoUpdate.Content };
    var validationResult = noteValidator.Validate(note);
    var isValidId = note.Id >= 1;

    if (!validationResult.IsValid || !isValidId)
        return Results.UnprocessableEntity(validationResult.Errors);

    var updatedNote = await logic.Update(note);
    var adaptedNote = updatedNote.Adapt<NoteDtoOut>();

    return Results.Ok(adaptedNote);
}).Produces<NoteDtoOut>()
  .Produces(422);

app.MapDelete("api/v1/notes/delete/{id}", async (long id, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var isValidId = id >= 1;
    if (!isValidId)
        return Results.NotFound();

    await logic.Delete(id);

    return Results.NoContent();
}).Produces(404)
  .Produces(204);

app.MapGet("api/v1/notes/list", async (ushort pageNumber, int itemsCount, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var noteList = await logic.GetPage(pageNumber, itemsCount);

    var page = noteList.Adapt<Page<NoteDtoOut>>();

    return Results.Ok(page);
}).Produces(200)
  .Produces<Page<NoteDtoOut>>();

app.MapPost("api/v1/notes/addTest", async (ushort count, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    foreach (var item in Enumerable.Range(0, count))
    {
        var note = new Note() { Title = item.ToString(), Content = Guid.NewGuid().ToString() };
        _ = await logic.Add(note);
    }

    return Results.NoContent();

}).Produces(201)
  .Produces(422);

app.Run();

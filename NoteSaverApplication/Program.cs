using Abdt.Loyal.NoteSaver.BusinessLogic;
using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.BusinessLogic.Validation;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.DTO;
using Abdt.Loyal.NoteSaver.Repository;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<NoteContext>(options => options.UseNpgsql(connection));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStorageLogic<Note>, StorageLogic>();
builder.Services.AddScoped<IValidator<Note>, Validator>();
builder.Services.AddScoped<IRepository<Note>, NoteDbRepository>();

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
    var isValidId = id > 1;

    if (!isValidId)
        return Results.NotFound();

    var note = await logic.GetNoteById(id);

    return Results.Ok(note);
}).Produces<Note>()
  .Produces(404);

app.MapPost("api/v1/notes/add", async (NoteDto noteDto, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var note = new Note() { Title = noteDto.Title, Content = noteDto.Content };

    if (!noteValidator.Validate(note).IsValid)
        return Results.UnprocessableEntity();

    var addedNote = await logic.AddNote(note);

    return Results.Created(new Uri("api/v1/notes/", UriKind.Relative), addedNote);
}).Produces(201)
  .Produces(422);

app.MapPut("api/v1/notes/update", async (NoteDtoUpdate noteDtoUpdate, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var note = new Note() { Id = noteDtoUpdate.Id, Title = noteDtoUpdate.Title, Content = noteDtoUpdate.Content };
    var validationResult = noteValidator.Validate(note);
    var isValidId = note.Id > 1;

    if (!validationResult.IsValid || !isValidId)
        return Results.UnprocessableEntity(validationResult.Errors);

    var updatedNote = await logic.UpdateNote(note);

    return Results.Ok(updatedNote);
}).Produces(200)
  .Produces(422);

app.MapDelete("api/v1/notes/delete/{id}", async (long id, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var isValidId = id > 1;
    if (!isValidId)
        return Results.NotFound();

    await logic.DeleteNote(id);

    return Results.NoContent();
}).Produces(404)
  .Produces(204);

app.MapGet("api/v1/notes/list", async (ushort pageNumber, int itemsCount, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    var noteList = await logic.GetAllNotes(pageNumber, itemsCount);

    return Results.Ok(noteList);
}).Produces(200)
  .Produces<Note>();

app.MapPost("api/v1/notes/addTest", async (ushort count, IStorageLogic<Note> logic, IValidator<Note> noteValidator) =>
{
    foreach (var item in Enumerable.Range(0, count))
    {
        var note = new Note() { Title = item.ToString(), Content = Guid.NewGuid().ToString() };
        _ = await logic.AddNote(note);
    }

    return Results.NoContent();

}).Produces(201)
  .Produces(422);

app.Run();

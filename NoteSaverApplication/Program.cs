using Abdt.Loyal.NoteSaver.BusinessLogic;
using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.DTO;
using Abdt.Loyal.NoteSaver.Repository;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStorageLogic<Note>, StorageLogic>();
builder.Services.AddScoped<INoteValidator, Validator>();

builder.Services.AddSingleton<IRepository<Note>, NoteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/v1/notes/{id}", (long id, IStorageLogic<Note> logic, INoteValidator noteValidator) => 
{ 
    if (!noteValidator.IsValidId(id))
        return Results.NotFound();

    var note = logic.GetNoteById(id);

    return Results.Ok(note);
}).Produces<Note>()
  .Produces(404);

app.MapPost("api/v1/notes/add", (NoteDto noteDto, IStorageLogic<Note> logic, INoteValidator noteValidator) =>
{
    var note = new Note() { Title = noteDto.Title, Content = noteDto.Content };

    if (!noteValidator.IsValid(note))
        return Results.UnprocessableEntity();
 
    var addedNote = logic.AddNote(note);

    return Results.Created(new Uri("api/v1/notes/", UriKind.Relative), addedNote);
}).Produces(201)
  .Produces(422);

app.MapPut("api/v1/notes/update", (NoteDtoUpdate noteDtoUpdate, IStorageLogic<Note> logic, INoteValidator noteValidator) =>
{
    var note = new Note() { Id = noteDtoUpdate.Id, Title = noteDtoUpdate.Title, Content = noteDtoUpdate.Content };

    if (!noteValidator.IsValid(note) || !noteValidator.IsValidId(note.Id))
        return Results.UnprocessableEntity();

    var updatedNote = logic.UpdateNote(note);

    return Results.Ok(updatedNote);
}).Produces(200)
  .Produces(422);

app.MapDelete("api/v1/notes/delete/{id}", (long id, IStorageLogic<Note> logic, INoteValidator noteValidator) =>
{
    if (!noteValidator.IsValidId(id))
        return Results.NotFound();

    logic.DeleteNote(id);

    return Results.NoContent();
}).Produces(404)
  .Produces(204);

app.MapGet("api/v1/notes/list", (IStorageLogic<Note> logic, INoteValidator noteValidator) =>
{
    var noteList = logic.GetAllNotes();

    return Results.Ok(noteList);
}).Produces(200)
  .Produces<Note>();

app.Run();

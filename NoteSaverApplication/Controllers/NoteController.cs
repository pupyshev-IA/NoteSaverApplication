using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.DTO;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Abdt.Loyal.NoteSaver.Controllers
{
    [ApiController]
    [Route("api/v1/notes")]
    public class NoteController : ControllerBase
    {
        private readonly IStorageLogic<Note> _service;
        private readonly IValidator<Note> _validator;

        public NoteController(IStorageLogic<Note> service, IValidator<Note> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetNoteById([FromRoute] long id)
        {
            var isValidId = id >= 1;

            if (!isValidId)
                return NotFound();

            var note = await _service.Get(id);
            var adaptedNote = note.Adapt<NoteDtoOut>();

            return Ok(adaptedNote);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNote([FromBody] NoteDto noteDto)
        {
            var note = new Note() { Title = noteDto.Title, Content = noteDto.Content };

            if (!_validator.Validate(note).IsValid)
                return UnprocessableEntity();

            var addedNoteId = await _service.Add(note);

            return Created(new Uri("api/v1/notes/", UriKind.Relative), addedNoteId);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateNote([FromBody] NoteDtoUpdate noteDtoUpdate)
        {
            var note = new Note() { Id = noteDtoUpdate.Id, Title = noteDtoUpdate.Title, Content = noteDtoUpdate.Content };
            var validationResult = _validator.Validate(note);
            var isValidId = note.Id >= 1;

            if (!validationResult.IsValid || !isValidId)
                return UnprocessableEntity(validationResult.Errors);

            var updatedNote = await _service.Update(note);
            var adaptedNote = updatedNote.Adapt<NoteDtoOut>();

            return Ok(adaptedNote);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteNote([FromQuery] long id)
        {
            var isValidId = id >= 1;
            if (!isValidId)
                return NotFound();

            await _service.Delete(id);

            return NoContent();
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetPage([FromQuery] ushort pageNumber, int itemsCount)
        {
            var noteList = await _service.GetPage(pageNumber, itemsCount);

            var page = noteList.Adapt<Page<NoteDtoOut>>();

            return Ok(page);
        }

        [HttpPost]
        [Route("addTest")]
        public async Task<IActionResult> AddTestNotes([FromQuery] ushort count)
        {
            foreach (var item in Enumerable.Range(0, count))
            {
                var note = new Note() { Title = $"NoteTitle: {item.ToString()}", Content = $"Content: text-{Guid.NewGuid().ToString()}" };
                _ = await _service.Add(note);
            }

            return NoContent();
        }
    }
}

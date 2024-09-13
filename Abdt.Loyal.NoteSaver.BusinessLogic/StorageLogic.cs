using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Options;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class StorageLogic : IStorageLogic<Note>
    {
        private readonly ILogger<StorageLogic> _logger;
        private readonly IRepository<Note> _repository;
        private readonly bool _isSoftDeleteEnabled;

        public StorageLogic(ILogger<StorageLogic> logger, IRepository<Note> repository, IOptions<LogicArgs> options)
        {
            _isSoftDeleteEnabled = options.Value.UseSoftDelete;
            _logger = logger;
            _repository = repository;
        }

        public async Task<Note> Add(Note note)
        {
            using var scope = _logger.BeginScope("");

            if (note == null)
                throw new ArgumentNullException(nameof(note));

            var addedNote = await _repository.Add(note);

            _logger.LogInformation("Added a note with id=\"{id}\" title=\"{title}\", content=\"{content}\"", addedNote, note.Title, note.Content);

            return addedNote;
        }

        public Task Delete(long id)
        {
            if (id <= 0)
            {
                _logger.LogError("Unable to delete a note with id=\"{id}\"", id);
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _logger.LogInformation("Deletion a note with id=\"{id}\"", id);

            return _repository.Delete(id);
        }

        public async Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount, long userId)
        {
            _logger.LogInformation("Getting a page number=\"{pageNumber}\" with \"{itemsCount}\" items", pageNumber, itemsCount);
            return await _repository.GetPage(pageNumber, itemsCount, userId);
        }

        public async Task<Note?> Get(long id, long userId)
        {
            if (id <= 0)
            {
                _logger.LogError("Unable to get a note with id=\"{id}\"", id);
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _logger.LogInformation("Getting specified note with id=\"{id}\"", id);
            return await _repository.GetById(id, userId);
        }

        public async Task<Note?> Update(Note note)
        {
            if (note == null)
            {
                _logger.LogError("Unable to update a note");
                throw new ArgumentNullException(nameof(note));
            }

            _logger.LogInformation("Updating a note: new title=\"{title}\", new content=\"{content}\"", note.Title, note.Content);

            return await _repository.Update(note);
        }

        //public Task RestoreDeleted(long id)
        //{
        //    return SoftDeleteOperation(id, true);
        //}
    }
}

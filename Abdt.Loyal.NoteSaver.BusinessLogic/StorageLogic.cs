using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class StorageLogic : IStorageLogic<Note>
    {
        private readonly IRepository<Note> _repository;

        public StorageLogic(IRepository<Note> repository)
        {
            _repository = repository;
        }

        public async Task<long> AddNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(nameof(note));

            return await _repository.Add(note);
        }

        public async Task DeleteNote(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            await _repository.Delete(id);
        }

        public async Task<Page<Note>> GetAllNotes(ushort pageNumber, int itemsCount)
        {
            return await _repository.GetPage(pageNumber, itemsCount);
        }

        public async Task<Note?> GetNoteById(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return await _repository.GetById(id);
        }

        public async Task<Note?> UpdateNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(nameof(note));

            return await _repository.Update(note);
        }
    }
}

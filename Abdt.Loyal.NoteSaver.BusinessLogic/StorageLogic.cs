using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class StorageLogic : IStorageLogic
    {
        private readonly IRepository<Note> _repository;

        public StorageLogic(IRepository<Note> repository)
        {
            _repository = repository;
        }

        public Note? AddNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(nameof(note));

            return _repository.GetById(_repository.Add(note));
        }

        public void DeleteNote(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            _repository.Delete(id);
        }

        public ICollection<Note> GetAllNotes()
        {
            return _repository.GetAllItems();
        }

        public Note? GetNoteById(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return _repository.GetById(id);
        }

        public Note? UpdateNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(nameof(note));

            return _repository.Update(note);
        }
    }
}
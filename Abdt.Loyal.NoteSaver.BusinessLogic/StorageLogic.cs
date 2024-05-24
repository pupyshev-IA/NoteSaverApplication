using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class StorageLogic : IStorageLogic
    {
        private readonly IRepository<Note> _repository;
        private readonly INoteValidator _noteValidator;

        public StorageLogic(IRepository<Note> repository, INoteValidator noteValidator)
        {
            _repository = repository;
            _noteValidator = noteValidator;
        }

        public Note AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public void DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        public Note FindNoteByContent(string content)
        {
            throw new NotImplementedException();
        }

        public Note FindNoteByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public ICollection<Note> GetAllNotes()
        {
            throw new NotImplementedException();
        }

        public Note UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
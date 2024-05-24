using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic
    {
        public ICollection<Note> GetAllNotes();
        public Note? AddNote(Note note);
        public Note? GetNoteById(long id);
        public Note? UpdateNote(Note note);
        public void DeleteNote(long id);
    }
}

using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic
    {
        public ICollection<Note> GetAllNotes();
        public Note FindNoteByTitle(string title);
        public Note FindNoteByContent(string content);
        public int AddNote(Note note);
        public Note UpdateNote(Note note);
        public bool DeleteNote(int id);
    }
}

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic<T>
    {
        Task<ICollection<T>> GetAllNotes();
        Task<long> AddNote(T note);
        Task<T?> GetNoteById(long id);
        Task<T?> UpdateNote(T note);
        Task DeleteNote(long id);
    }
}

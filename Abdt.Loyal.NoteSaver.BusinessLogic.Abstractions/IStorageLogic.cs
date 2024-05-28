using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic<T>
    {
        Task<Page<T>> GetAllNotes(ushort pageNumber, int itemsCount);
        Task<T> AddNote(T note);
        Task<T?> GetNoteById(long id);
        Task<T?> UpdateNote(T note);
        Task DeleteNote(long id);
    }
}

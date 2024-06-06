using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic<T>
    {
        Task<Page<T>> GetPage(ushort pageNumber, int itemsCount);
        Task<long> Add(T note);
        Task<T?> Get(long id);
        Task<T?> Update(T note);
        Task Delete(long id);
        Task RestoreDeleted(long id);
    }
}

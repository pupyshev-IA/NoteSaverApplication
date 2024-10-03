using Abdt.Loyal.NoteSaver.Web.Shared;

namespace Abdt.Loyal.NoteSaver.Web.Client.Http
{
    public interface INoteClient<T>
    {
        Task<T> Add(T note);

        Task<T?> Get(long id);

        Task<Page<T>> GetPage(ushort pageNumber, int itemsCount);

        Task<T?> Update(T note);

        Task Delete(long id);
    }
}

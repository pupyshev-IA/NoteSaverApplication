namespace Abdt.Loyal.NoteSaver.Repository.Abstractions
{
    public interface ICache
    {
        void Set<T>(string id, T item) where T : new();

        Task<T?> Get<T>(string id) where T : class, new();

        void Invalidate<T>(string id) where T : new();
    }
}
